using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Project.DesktopClient.DTO.Review;
using Project.DesktopClient.Services.Api;
using Project.DesktopClient.Services.Infrastructure;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.ViewModels.Review.Dialogs
{
    public class ReviewCreateDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ApiClient _apiClient;
        private IBusyService _busyService;
        private readonly IOpenFileDialogService _fileDialogService;


        private string _title = "Criar Review";

        private string _reviewTitle = string.Empty;
        private string _category = string.Empty;
        private string _description = string.Empty;
        private int _userId;
        private string _imagePath = string.Empty;

        private BitmapImage? _imagePreview;

        #region Encapsulations
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string ReviewTitle
        {
            get => _reviewTitle;
            set => SetProperty(ref _reviewTitle, value);
        }
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }
        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }
        public BitmapImage? ImagePreview
        {
            get => _imagePreview;
            set => SetProperty(ref _imagePreview, value);
        }
        #endregion


        // Commands
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SelectImageCommand { get; }
        public DelegateCommand CreateReviewCommand { get; }


        public ReviewCreateDialogViewModel(ApiClient apiClient, IBusyService busyService, IOpenFileDialogService fileDialogService)
        {
            _apiClient = apiClient;
            _busyService = busyService;
            _fileDialogService = fileDialogService;

            CancelCommand = new DelegateCommand(Cancel);
            SelectImageCommand = new DelegateCommand(SelectImage);

            CreateReviewCommand = new DelegateCommand(async () => await CreateReviewAsync());

        }
        private void SelectImage()
        {
            var path = _fileDialogService.ShowImageDialog();
            if (string.IsNullOrWhiteSpace(path))
                return;

            ImagePath = path;

            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(path);
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.EndInit();

            ImagePreview = bmp;
        }

        private async Task CreateReviewAsync()
        {
            _busyService.IsBusy = true;

            var review = new ReviewCreateDto
            {
                Title = ReviewTitle,
                Category = Category,
                Description = Description,
                UserId = UserId,
                ImageFilePath = ImagePath

            };

            await _apiClient.PostReviewAsync(review);

            _busyService.IsBusy = false;

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }
    }
}
