using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Project.DesktopClient.DTO.User;
using Project.DesktopClient.Services.Api;
using Project.DesktopClient.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.ViewModels.User
{
    public class UserMainViewModel : BindableBase
    {
        private readonly ApiClient _apiClient;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private IBusyService _busyService;

        private string _title = "Menu de Usuários";

        private ObservableCollection<UserDetailsDto>? _users;

        private int _id;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private int _age;
        private string _email = string.Empty;

        #region Encapsulations
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public ObservableCollection<UserDetailsDto> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        #endregion

        // Commands
        public DelegateCommand NavigateMenuCommand { get; }
        public DelegateCommand LoadUsersCommand { get; }
        public DelegateCommand OpenCreateDialogCommand { get; }


        public UserMainViewModel(ApiClient apiClient, IRegionManager regionManager, IDialogService dialogService, IBusyService busyService)
        {
            _apiClient = apiClient;
            _regionManager = regionManager;
            _dialogService = dialogService;
            _busyService = busyService;

            Users = new ObservableCollection<UserDetailsDto>();

            NavigateMenuCommand = new DelegateCommand(NavigateToMenu);
            OpenCreateDialogCommand = new DelegateCommand(OpenCreateDialog);
            LoadUsersCommand = new DelegateCommand(async () => await LoadUsersAsync());

            _ = LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            _busyService.IsBusy = true;

            Users.Clear();

            var users = await _apiClient.GetUsersAsync();
            foreach (var user in users)
            {
                Users.Add(user);
            }

            _busyService.IsBusy = false;
        }

        // Navigation
        private void NavigateToMenu()
        {
            _regionManager.RequestNavigate("ShellRegion", "MainMenuView");
        }

        private void OpenCreateDialog()
        {
            _busyService.IsBusy = true;

            _dialogService.ShowDialog("UserCreateDialog");

            _busyService.IsBusy = false;
        }
    }
}
