using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.ViewModels.Shell
{
    public class MainMenuViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand NavigateUserCommand { get; }
        public DelegateCommand NavigateReviewCommand { get; }

        public MainMenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateUserCommand = new DelegateCommand(NavigateToUser);
            NavigateReviewCommand = new DelegateCommand(NavigateToReview);
        }

        private void NavigateToUser()
        {
            _regionManager.RequestNavigate("ShellRegion", "UserMainView");
        }

        private void NavigateToReview()
        {
            _regionManager.RequestNavigate("ShellRegion", "ReviewMainView");
        }
    }
}
