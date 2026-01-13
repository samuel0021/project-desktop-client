using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Project.DesktopClient.Services.Infrastructure
{
    public class OpenFileDialogService : IOpenFileDialogService
    {
        public string? ShowImageDialog()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Imagens|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Multiselect = false
            };

            return dlg.ShowDialog() == true ? dlg.FileName : null;
        }
    }
}
