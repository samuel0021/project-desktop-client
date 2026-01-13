using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Project.DesktopClient.Services.Api;
using Project.DesktopClient.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Project.DesktopClient.ViewModels.Review.Dialogs
{
    public class ReviewVisualizeDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ApiClient _apiClient;
        private IBusyService _busyService;

        private string _title = "Visualizar Review";

        private int _id;
        private string _reviewTitle = string.Empty;
        private string _category = string.Empty;
        private string _description = string.Empty;
        private int _userId;
        private string _imagePath = string.Empty;

        private BitmapImage? _imagePreview;

        #region Encapsulations
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
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

        public ReviewVisualizeDialogViewModel(ApiClient apiclient, IBusyService busyService)
        {
            _apiClient = apiclient;
            _busyService = busyService;

            CancelCommand = new DelegateCommand(Cancel);
        }
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        public event Action<IDialogResult>? RequestClose;

        public async void OnDialogOpened(IDialogParameters parameters) 
        {
            if (parameters.ContainsKey("reviewId"))
            {
                 Id = parameters.GetValue<int>("reviewId");
            }

            _busyService.IsBusy = true;

            var review = await _apiClient.GetReviewById(Id);

            if (review is not null)
            {
                ReviewTitle = review.Title;
                Category = review.Category;
                Description = review.Description;
                UserId = review.UserId;
                ImagePath = review.ImageUrl ?? string.Empty;

                LoadImagePreview();
            }

            _busyService.IsBusy = false;
        }

        private void LoadImagePreview()
        {
            if (string.IsNullOrWhiteSpace(ImagePath))
            {
                ImagePreview = null;
                return;
            }

            // base da API, por ex. https://localhost:5001/
            var baseUri = _apiClient.BaseAddress; // tipo Uri

            // se ImagePath vier com "/", ex.: "/reviews/uploads/x.jpg"
            var uri = new Uri(baseUri, ImagePath); // overload Uri(Uri baseUri, string relativeUri) [1]

            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = uri;
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.EndInit();

            ImagePreview = bmp;
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

    }
}
