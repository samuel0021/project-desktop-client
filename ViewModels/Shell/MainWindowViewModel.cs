using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using Project.DesktopClient.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.ViewModels.Shell
{
    public class MainWindowViewModel : BindableBase
    {
        public IBusyService BusyService { get; }

        public MainWindowViewModel(IBusyService busyService)
        {
            BusyService = busyService;
        }
    }
}
