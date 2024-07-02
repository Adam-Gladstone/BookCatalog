using System.Diagnostics;
using BookCatalog.Contracts.ViewModels;
using BookCatalog.Core.Contracts.Services;
using BookCatalog.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;

namespace BookCatalog.ViewModels;

public partial class BookCatalogViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private string filter = "";

    [ObservableProperty]
    private string itemCount;

    private BookItem? current;

    public BookItem? Current
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }

    public bool HasCurrent => current is not null;

    public BookCatalogViewModel(IDataService dataService)
    {
        _dataService = dataService;

        _dataService.InitializeDataAsync();
    }

    public async Task<List<BookItem>> GetDataAsync()
    {
        var books = (List<BookItem>)await _dataService.GetItemsAsync();

        return books;
    }

    public void OnNavigatedTo(object parameter)
    {

    }

    public void OnNavigatedFrom()
    {
        // Run code when the app navigates away from this page
    }

    public void AddBookItems(IReadOnlyList<StorageFile> files)
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

            var id = _dataService.AddItemAsync(entry).Result;
            Debug.WriteLine($"Added item \'{entry.Title}\' with id:{id}");
        }
    }

    public void OpenBookItem(BookItem item)
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
            Debug.WriteLine($"The referenced file ({filename}) does not exist in this location.");
        }
    }

    public void DeleteBookItem()
    {
        if (Current != null)
        {
            var id = _dataService.DeleteItemAsync(Current).Result;
            Debug.WriteLine($"Deleted item \'{Current.Title}\' with id:{id}");
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
