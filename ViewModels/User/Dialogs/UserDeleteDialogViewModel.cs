using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Project.DesktopClient.Services.Api;
using Project.DesktopClient.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.ViewModels.User.Dialogs
{
    public class UserDeleteDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ApiClient _apiClient;
        private IBusyService _busyService;

        private string _title = "Deletar Usuário";

        private int _userId;

        private int _id;
        private string _firstName;
        private string _lastName;
        private int _age;
        private string _email;

        #region Encapsulations
        public string Title
        {
            get => _title;
            set => SetProperty(ref  _title, value);
        }
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
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
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand DeleteUserCommand { get; }

        public UserDeleteDialogViewModel(ApiClient apiClient, IBusyService busyService)
        {
            _apiClient = apiClient;
            _busyService = busyService;

            CancelCommand = new DelegateCommand(Cancel);
            DeleteUserCommand = new DelegateCommand(async () => await DeleteUserAsync());
        }
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private async Task DeleteUserAsync()
        {
            _busyService.IsBusy = true;

            await _apiClient.DeleteUserAsync(UserId);

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));

            _busyService.IsBusy = false;
        }

        public event Action<IDialogResult>? RequestClose;
        public async void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = parameters.GetValue<int>("userId");
            }

            _busyService.IsBusy = true;

            var user = await _apiClient.GetUserByIdAsync(UserId);

            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Age = user.Age;
            Email = user.Email;

            _busyService.IsBusy = false;
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }
    }
}
