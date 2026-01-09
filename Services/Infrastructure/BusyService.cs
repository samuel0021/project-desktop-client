using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.Services.Infrastructure
{
    public class BusyService : BindableBase, IBusyService
    {
        private bool _isBusy;

        public bool IsBusy 
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
    }
}
