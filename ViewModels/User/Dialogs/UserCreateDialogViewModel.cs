using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Project.DesktopClient.DTO.User;
using Project.DesktopClient.Services.Api;
using Project.DesktopClient.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.ViewModels.User.Dialogs
{
    public class UserCreateDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ApiClient _apiClient;
        private IBusyService _busyService;

        private string _title = "Cadastrar Usuário";

        private string? _firstName;
        private string? _lastName;
        private int _age;
        private string? _email;

        #region Encapsulations
        public string Title 
        {
            get => _title;
            set => SetProperty(ref  _title, value);
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
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand CreateUserCommand { get; }

        public UserCreateDialogViewModel(ApiClient apiClient, IBusyService busyService)
        {
            _apiClient = apiClient;
            _busyService = busyService;

            CreateUserCommand = new DelegateCommand(async () => await CreateUserAsync());
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            // "Voltar/Cancelar" → só fecha o diálogo
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private async Task CreateUserAsync()
        {
            _busyService.IsBusy = true;

            var user = new UserCreateDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Age = Age,
                Email = Email
            };

            await _apiClient.PostUserAsync(user);

            _busyService.IsBusy = false;

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }


        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }
    }
}
