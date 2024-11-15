﻿using System.Diagnostics;
using System.Threading.Tasks;
using BookCatalog.Activation;
using BookCatalog.Contracts.Services;
using BookCatalog.Core.Contracts.Services;
using BookCatalog.Core.Services;
using BookCatalog.Dialogs;
using BookCatalog.Models;
using BookCatalog.Services;
using BookCatalog.ViewModels;
using BookCatalog.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WASDK = Microsoft.WindowsAppSDK;

namespace BookCatalog;

public partial class App : Application
{
    public static FrameworkElement? MainRoot
    {
        get; private set;
    }

    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IFolderSelectorService, FolderSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IDataService, SqliteDataService>();
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<BookCatalogViewModel>();
            services.AddTransient<BookCatalogPage>();

            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.

        if (e.Exception != null)
        {
            Debug.WriteLine(e.Exception.ToString());
        }
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        MainRoot = MainWindow.Content as FrameworkElement;

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
    public static string WinAppSdkDetails => string.Format("Windows App SDK {0}.{1}", WASDK.Release.Major, WASDK.Release.Minor);

    public static string WinAppSdkRuntimeDetails
    {
        get
        {
            try
            {
                // Retrieve Windows App Runtime version info dynamically
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Possible null reference argument.
                var windowsAppRuntimeVersion =
                    from module in Process.GetCurrentProcess().Modules.OfType<ProcessModule>()
                    where module.FileName.EndsWith("Microsoft.WindowsAppRuntime.Insights.Resource.dll")
                    select FileVersionInfo.GetVersionInfo(module.FileName);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Possible null reference argument.
                return WinAppSdkDetails + ", Windows App Runtime " + windowsAppRuntimeVersion.First().FileVersion;
            }
            catch
            {
                return WinAppSdkDetails + $", Windows App Runtime {WASDK.Runtime.Version.Major}.{WASDK.Runtime.Version.Minor}";
            }
        }
    }

    // Generic exception reporting
    public static async void ReportException(Exception e)
    {
        var themeSelectorService = App.GetService<IThemeSelectorService>();

        ContentDialog dialog = new()
        {
            XamlRoot = MainWindow.Content.XamlRoot,
            RequestedTheme = themeSelectorService.Theme,
            Title = "Exception Report",
            PrimaryButtonText = "OK",
            DefaultButton = ContentDialogButton.Primary,

            Content = new ExceptionDialog(e)
        };

        _ = await dialog.ShowAsync();
    }
}
