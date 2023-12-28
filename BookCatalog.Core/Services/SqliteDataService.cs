using BookCatalog.Core.Contracts.Services;
using BookCatalog.Core.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;


namespace BookCatalog.Core.Services;
public class SqliteDataService : IDataService
{
    private string dbPath = @"D:\Development\Projects\C#\BookCatalog\Database\bookCollectionData.db";

    public string DbPath
    {
        get => dbPath;
        set => dbPath = value;
    }

    public async Task InitializeDataAsync()
    {
        using var db = GetOpenConnectionAsync();
        await CreateBookItemTableAsync(db);
    }

    public async Task<int> AddItemAsync(BookItem item)
    {
        using var db = GetOpenConnectionAsync();
        return await InsertBookItemAsync(db, item);
    }

    public async Task<BookItem> GetItemAsync(int id)
    {
        IList<BookItem> bookItems;

        using (var db = GetOpenConnectionAsync())
        {
            bookItems = await GetAllBookItemsAsync(db);
        }

        // Filter the list to get the item for our Id.
        return bookItems.FirstOrDefault(i => i.Id == id);
    }

    public async Task<IList<BookItem>> GetItemsAsync()
    {
        using var db = GetOpenConnectionAsync();
        return await GetAllBookItemsAsync(db);
    }

    public async Task<bool> DeleteItemAsync(BookItem item)
    {
        using var db = GetOpenConnectionAsync();
        return await DeleteBookItemAsync(db, item.Id);
    }

    public async Task UpdateItemAsync(BookItem item)
    {
        using var db = GetOpenConnectionAsync();
        await UpdateBookItemAsync(db, item);
    }

    private SqliteConnection GetOpenConnectionAsync()
    {
        var connection = new SqliteConnection($"Filename={DbPath}");
        connection.Open();

        return connection;
    }

    private async Task CreateBookItemTableAsync(SqliteConnection db)
    {
        var tableCommand = @"CREATE TABLE IF NOT 
                EXISTS BookItems (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                Category TEXT,
                Title TEXT NOT NULL, 
                Path TEXT NOT NULL,                
                UsageCount INTEGER NOT NULL
                )";

        var createTable = new SqliteCommand(tableCommand, db);

        await createTable.ExecuteNonQueryAsync();
    }

    private async Task<List<BookItem>> GetAllBookItemsAsync(SqliteConnection db)
    {
        var itemsResult = await db.QueryAsync<BookItem>
                        (
                            @"SELECT
                                    [BookItems].[Id],
                                    [BookItems].[Category],
                                    [BookItems].[Title],
                                    [BookItems].[Path],
                                    [BookItems].[UsageCount]
                                FROM
                                    [BookItems]"
                        );

        return itemsResult.ToList();
    }

    private async Task<int> InsertBookItemAsync(SqliteConnection db, BookItem item)
    {
        var newIds = await db.QueryAsync<long>(
            @"INSERT INTO BookItems
                    (Category, Title, Path, UsageCount)
                    VALUES
                    (@Category, @Title, @Path, @UsageCount);
                SELECT last_insert_rowid()", item);

        return (int)newIds.First();
    }

    private async Task UpdateBookItemAsync(SqliteConnection db, BookItem item)
    {
        await db.QueryAsync(
            @"UPDATE BookItems
                  SET Category = @Category,
                      Title = @Title,
                      Path = @Path,
                      UsageCount = @UsageCount
                  WHERE Id = @Id;", item);
    }

    private async Task<bool> DeleteBookItemAsync(SqliteConnection db, int id)
    {
        // This will look in the table BookItems
        return await db.DeleteAsync<BookItem>(new BookItem { Id = id });
    }
}
