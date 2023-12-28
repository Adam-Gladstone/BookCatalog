using BookCatalog.Core.Models;

namespace BookCatalog.Core.Contracts.Services;
public interface IDataService
{
    Task InitializeDataAsync();

    Task<int> AddItemAsync(BookItem item);

    Task<BookItem> GetItemAsync(int id);

    Task<IList<BookItem>> GetItemsAsync();

    Task<bool> DeleteItemAsync(BookItem item);

    Task UpdateItemAsync(BookItem item);

}
