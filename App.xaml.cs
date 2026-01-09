using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using Project.DesktopClient.Services.Api;
using Project.DesktopClient.Services.Infrastructure;
using Project.DesktopClient.ViewModels.Shell;
using Project.DesktopClient.ViewModels.User;
using Project.DesktopClient.ViewModels.User.Dialogs;
using Project.DesktopClient.Views.Shell;
using Project.DesktopClient.Views.User;
using Project.DesktopClient.Views.User.Dialogs;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Project.DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var regionManager = Container.Resolve<IRegionManager>();

            regionManager.RequestNavigate("ShellRegion", "MainMenuView");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // API
            containerRegistry.RegisterSingleton<ApiClient>(() =>
                new ApiClient("https://localhost:7118"));

            // Interfaces
            containerRegistry.RegisterSingleton<IBusyService, BusyService>();

            // Navigation
            containerRegistry.RegisterForNavigation<MainMenuView, MainMenuViewModel>("MainMenuView");
            containerRegistry.RegisterForNavigation<UserMainView, UserMainViewModel>("UserMainView");

            // Dialogs
            containerRegistry.RegisterDialog<UserCreateDialogView, UserCreateDialogViewModel>("UserCreateDialog");
            containerRegistry.RegisterDialog<UserEditDialogView, UserEditDialogViewModel>("UserEditDialog");
            containerRegistry.RegisterDialog<UserDeleteDialogView, UserDeleteDialogViewModel>("UserDeleteDialog");
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            ViewModelLocationProvider.Register<MainMenuView, MainMenuViewModel>();
        }
    }

}
