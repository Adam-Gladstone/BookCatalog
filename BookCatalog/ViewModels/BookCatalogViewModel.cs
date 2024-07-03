using System.Diagnostics;
using BookCatalog.Core.Contracts.Services;
using BookCatalog.Core.Models;
using BookCatalog.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;

namespace BookCatalog.ViewModels;

public partial class BookCatalogViewModel : ObservableRecipient
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private string filter = "";

    [ObservableProperty]
    private string? itemCount;

    [ObservableProperty]
    private BookItem? selected;

    public BookCatalogViewModel(IDataService dataService)
    {
        _dataService = dataService;

        var settings = App.GetService<SettingsViewModel>();

        if (!string.IsNullOrEmpty(settings.DatabaseFolder))
        {
            var sqliteDataService = (SqliteDataService)_dataService;

            if(Directory.Exists(settings.DatabaseFolder))
            {
                sqliteDataService.DbPath = Path.Combine(settings.DatabaseFolder, sqliteDataService.DbFilename);
            }
            else
            {
                sqliteDataService.DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteDataService.DbFilename);
            }

            _dataService.InitializeDataAsync();
        }
    }

    public async Task<List<BookItem>> GetDataAsync()
    {
        return (List<BookItem>)await _dataService.GetItemsAsync();
    }

    public void AddBookItems(IReadOnlyList<StorageFile> files)
    {
        try
        {
            foreach (var file in files)
            {
                var entry = new BookItem()
                {
                    Category = GetCategoryFromDirectory(file),
                    Title = file.Name,
                    Path = file.Path,
                    UsageCount = 0
                };

                _ = _dataService.AddItemAsync(entry).Result;
            }
        }
        catch (Exception ex)
        {
            App.ReportException(ex);
        }
    }

    public void OpenBookItem(BookItem item)
    {
        try
        {
            var filename = item.Path;
            if (File.Exists(filename))
            {
                new Process
                {
                    StartInfo = new ProcessStartInfo(filename)
                    {
                        UseShellExecute = true
                    }
                }.Start();
            }
            else
            {
                throw new Exception($"The referenced file ({filename}) does not exist in this location.");
            }
        }
        catch (Exception ex)
        {
            App.ReportException(ex);
        }
    }

    public void DeleteBookItem()
    {
        try
        {
            if (Selected != null)
            {
                var id = _dataService.DeleteItemAsync(Selected).Result;
                if (id != false)
                {
                    Debug.WriteLine($"Deleted item \'{Selected.Title}\' with id:{id}");
                }
                else
                {
                    throw new Exception($"Unable to delete the item \'{Selected.Title}\'.");
                }
            }
        }
        catch(Exception ex)
        {
            App.ReportException(ex);
        }
    }

    private static string GetCategoryFromDirectory(StorageFile file)
    {
        var category = "";
        var directories = file.Path.Split(Path.DirectorySeparatorChar);
        if (directories.Length > 0)
        {
            category = directories[^2] ?? "";
        }
        return category;
    }
}
