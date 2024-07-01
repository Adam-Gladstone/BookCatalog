using System.Collections.ObjectModel;
using System.Diagnostics;
using BookCatalog.Dialogs;
using BookCatalog.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage.Pickers;

namespace BookCatalog.Views;

public enum Group
{
    None,
    Title,
    Category
}

public class GroupedList : List<object>
{
    public GroupedList(IEnumerable<object> items) : base(items)
    {
    }
    public object? Key { get; set; }

    public override string ToString()
    {
        return $"Group {Key?.ToString()}";
    }
}

public sealed partial class BookCatalogPage : Page
{
    private Group mGroup = Group.Title;

    public BookCatalogViewModel ViewModel { get; }

    public BookCatalogPage()
    {
        ViewModel = App.GetService<BookCatalogViewModel>();
        
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        UpdateGroupSource(ViewModel.Filter);
    }

    private async void UpdateGroupSource(string? filter)
    {
        switch (mGroup)
        {
            case Group.Title:
                BookInfoCVS.Source = await GetTitleDataAsync(filter);
                break;
            case Group.Category:
                BookInfoCVS.Source = await GetCategoryDataAsync(filter);
                break;
        }
    }

    public async Task<ObservableCollection<GroupedList>> GetTitleDataAsync(string? filter)
    {
        var books = await ViewModel.GetDataAsync();

        IEnumerable<GroupedList> groupedList;

        if (string.IsNullOrEmpty(filter))
        {
            groupedList = from book in books
                          group book by book.Title[..1] into g
                          orderby g.Key
                          select new GroupedList(g) { Key = g.Key };

        }
        else
        {
            groupedList = from book in books
                          where book.Title.Contains(filter)
                          group book by book.Title[..1] into g
                          orderby g.Key
                          select new GroupedList(g) { Key = g.Key };
        }

        return new ObservableCollection<GroupedList>(groupedList);
    }

    public async Task<ObservableCollection<GroupedList>> GetCategoryDataAsync(string? filter)
    {
        var books = await ViewModel.GetDataAsync();

        IEnumerable<GroupedList> groupedList;

        if (string.IsNullOrEmpty(filter))
        {
            groupedList = from book in books
                          group book by book.Category into g
                          orderby g.Key
                          select new GroupedList(g) { Key = g.Key };

        }
        else
        {
            groupedList = from book in books
                          where book.Title.Contains(filter)
                          group book by book.Category into g
                          orderby g.Key
                          select new GroupedList(g) { Key = g.Key };
        }

        return new ObservableCollection<GroupedList>(groupedList);
    }


    private void ListView_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
    {
        if (ListView.SelectedItem is Core.Models.BookItem book)
        {
            Debug.WriteLine($"Selected item:{book.Category}, {book.Title}");
            ViewModel.OpenBookItem(book);
        }
    }

    private void ButtonTitles_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        mGroup = Group.Title;

        UpdateGroupSource(ViewModel.Filter);
    }

    private void ButtonCategory_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        mGroup = Group.Category;

        UpdateGroupSource(ViewModel.Filter);
    }

    public async void AddBookItems()
    {
        try
        {
            FileOpenPicker picker = new();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            picker.FileTypeFilter.Add("*");

            var files = await picker.PickMultipleFilesAsync();

            if (files != null && files.Count > 0)
            {
                ViewModel.AddBookItems(files);

                SearchBox.Text = string.Empty;
                ViewModel.Filter = "";

                UpdateGroupSource(ViewModel.Filter);
            }
        }
        catch (Exception e)
        {
            ReportException(e);
        }
    }

    public void DeleteBookItem()
    {
        try
        {
            ViewModel.DeleteBookItem();

            SearchBox.Text = string.Empty;
            ViewModel.Filter = "";

            UpdateGroupSource(ViewModel.Filter);
        }
        catch (Exception e)
        {
            ReportException(e);
        }
    }

    private async void ReportException(Exception e)
    {
        ContentDialog dialog = new()
        {
            XamlRoot = XamlRoot,
            Style = App.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Exception Report",
            PrimaryButtonText = "OK",
            DefaultButton = ContentDialogButton.Primary,
            Content = new ExceptionDialog(e)
        };

        _ = await dialog.ShowAsync();
    }

    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        UpdateGroupSource(args.QueryText);
    }

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            UpdateGroupSource(sender.Text);
        }
    }

    private void ButtonAdd_Click(object? sender, RoutedEventArgs? e)
    {
        AddBookItems();
    }

    private void ButtonDelete_Click(object? sender, RoutedEventArgs? e)
    {
        DeleteBookItem();
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }
}
