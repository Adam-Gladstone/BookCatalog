using System.Diagnostics;
using BookCatalog.Dialogs;
using BookCatalog.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Storage.Pickers;

namespace BookCatalog.Views;

public sealed partial class CatalogPage : Page
{
    public CatalogViewModel ViewModel
    {
        get;
    }

    public CatalogPage()
    {
        ViewModel = App.GetService<CatalogViewModel>();

        InitializeComponent();

        gridControl.AddHandler(FrameworkElement.DoubleTappedEvent, new DoubleTappedEventHandler(GridControl_DoubleTapped), true);
    }

    private void GridControl_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        if (gridControl.SelectedItem is Core.Models.BookItem bookItem)
        {
            Debug.WriteLine($"Selected item:{bookItem.Category}, {bookItem.Title}");

            ViewModel.OpenBookItem(bookItem);
        }
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

                ViewModel.LoadBookCollection();
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

            ViewModel.LoadBookCollection();
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
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Exception Report",
            PrimaryButtonText = "OK",
            DefaultButton = ContentDialogButton.Primary,
            Content = new ExceptionDialog(e)
        };

        _ = await dialog.ShowAsync();
    }
}
