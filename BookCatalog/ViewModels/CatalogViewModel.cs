using System.Collections.ObjectModel;
using System.Diagnostics;
using BookCatalog.Contracts.ViewModels;
using BookCatalog.Core.Contracts.Services;
using BookCatalog.Core.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.Storage;

namespace BookCatalog.ViewModels;

public partial class CatalogViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<BookItem> collection = new();

    [ObservableProperty]
    private BookItem selectedBookItem = new();

    public CatalogViewModel(IDataService dataService)
    {
        _dataService = dataService;

        _dataService.InitializeDataAsync();
    }

    public async void OnNavigatedTo(object parameter)
    {
        Collection.Clear();

        var items = await _dataService.GetItemsAsync();

        foreach (var item in items)
        {
            Collection.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
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
            // 
        }
    }

    public void DeleteBookItem()
    {
        if (SelectedBookItem != null)
        {
            var id = _dataService.DeleteItemAsync(SelectedBookItem).Result;
            Debug.WriteLine($"Deleted item \'{SelectedBookItem.Title}\' with id:{id}");
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

    public async void LoadBookCollection()
    {
        Collection.Clear();

        var items = await _dataService.GetItemsAsync();

        foreach (var item in items)
        {
            Collection.Add(item);
        }
    }
}
