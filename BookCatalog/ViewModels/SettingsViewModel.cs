using System.Reflection;
using System.Windows.Input;

using BookCatalog.Contracts.Services;
using BookCatalog.Helpers;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using Windows.Storage.Pickers;

namespace BookCatalog.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    private readonly IFolderSelectorService _folderSelectorService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private string _databaseFolder;

    public RelayCommand SetFolderCommand
    {
        get; set;
    }

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IFolderSelectorService folderSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _folderSelectorService = folderSelectorService;

        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });

        SetFolderCommand = new RelayCommand(SetFolder);

        _databaseFolder = _folderSelectorService.Folder;
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    private async void SetFolder()
    {
        try
        {
            FolderPicker picker = new();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            picker.CommitButtonText = "Select this folder";

            var folderItem = await picker.PickSingleFolderAsync();
            if (folderItem != null)
            {
                DatabaseFolder = folderItem.Path;

                var folderSelectorService = App.GetService<IFolderSelectorService>();
                await folderSelectorService.SetFolderAsync(folderItem.Path);
            }
        }
        catch (Exception ex)
        {
            App.ReportException(ex);
        }
    }
}
