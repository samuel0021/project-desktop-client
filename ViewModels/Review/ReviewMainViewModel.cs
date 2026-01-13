using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Project.Api.DTO.Reviews;
using Project.DesktopClient.Services.Api;
using Project.DesktopClient.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.ViewModels.Review
{
    public class ReviewMainViewModel : BindableBase
    {
        private readonly ApiClient _apiClient;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private IBusyService _busyService;

        private ObservableCollection<ReviewDetailsDto>? _reviews;
        private ReviewDetailsDto? _selectedReview;


        private int _id;
        private string _title = string.Empty;
        private string _category = string.Empty;
        private string _description = string.Empty;
        private int _userId;
        private string _userName = string.Empty;


        #region Encapsulations
        public ObservableCollection<ReviewDetailsDto> Reviews
        {
            get => _reviews;
            set => SetProperty(ref _reviews, value);
        }
        public ReviewDetailsDto? SelectedReview
        {
            get => _selectedReview;
            set => SetProperty(ref _selectedReview, value);
        }
        public int Id
        {
            get => _id;
            set =>  SetProperty(ref _id, value);
        }
        public string Title
        {
            get => _title;
            set =>  SetProperty(ref _title, value);
        }
        public string Category
        {
            get => _category;
            set =>  SetProperty(ref _category, value);
        }
        public string Description
        {
            get => _description;
            set =>  SetProperty(ref _description, value);
        }
        public int UserId
        {
            get => _userId;
            set =>  SetProperty(ref _userId, value);
        }
        public string UserName
        {
            get => _userName;
            set =>  SetProperty(ref _userName, value);
        }
        #endregion

        // Commands
        public DelegateCommand NavigateMenuCommand { get; }
        public DelegateCommand LoadReviewsCommand { get; }
        public DelegateCommand OpenVisualizeDialogCommand { get; }
        public DelegateCommand OpenCreateDialogCommand { get; }


        public ReviewMainViewModel(ApiClient apiClient, IRegionManager regionManager, IDialogService dialogService, IBusyService busyService)
        {
            _apiClient = apiClient;
            _regionManager = regionManager;
            _dialogService = dialogService;
            _busyService = busyService;

            Reviews = new ObservableCollection<ReviewDetailsDto>();

            NavigateMenuCommand = new DelegateCommand(NavigateToMenu);

            OpenVisualizeDialogCommand = new DelegateCommand(OpenReviewDialog);
            OpenCreateDialogCommand = new DelegateCommand(OpenCreateDialog);

            LoadReviewsCommand = new DelegateCommand(async () => await LoadReviewsAsync());

            _ = LoadReviewsAsync();
        }

        // Navigation
        private void NavigateToMenu()
        {
            _regionManager.RequestNavigate("ShellRegion", "MainMenuView");
        }

        // VISUALIZE DIALOG
        private void OpenReviewDialog()
        {
            if (SelectedReview == null)
                return;

            _busyService.IsBusy = true;

            var parameters = new DialogParameters
            {
                { "reviewId", SelectedReview.Id },
            };

            _dialogService.ShowDialog("ReviewVisualizeDialog", parameters, r =>
            {
                // Atualiza lista ao editar usuário
                if (r.Result == ButtonResult.OK)
                {
                    _ = LoadReviewsAsync();
                }
            });

            _busyService.IsBusy = false;
        }

        // CREATE DIALOG
        private void OpenCreateDialog()
        {
            _busyService.IsBusy = true;

            _dialogService.ShowDialog("ReviewCreateDialog", r =>
            {
                // Atualiza lista ao cadastrar usuário
                if (r.Result == ButtonResult.OK)
                {
                    _ = LoadReviewsAsync();
                }
            });

            _busyService.IsBusy = false;
        }

        // API
        private async Task LoadReviewsAsync()
        {
            _busyService.IsBusy = true;

            Reviews.Clear();

            var reviews = await _apiClient.GetReviewsAsync();
            foreach (var review in reviews)
            {
                Reviews.Add(review);
            }

            _busyService.IsBusy = false;
        }
    }
}
