using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.Services.Infrastructure
{
    public interface IBusyService
    {
        bool IsBusy { get; set; }
    }
}
