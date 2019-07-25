using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSharedLibrary;

namespace irc_core.Dialogs
{
    public class Dialog : ObservableObject
    {
        public async void Show()
        {
            await DialogHost.Show(this);
        }
    }
}
