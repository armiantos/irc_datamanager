using irc_datamanager.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_datamanager.Models
{
    class WinAppModel : ObservableObject
    {
        private DataView handlesView;
        private string currentWindow;
        private string childWindowsVisibility;

        public string CurrentWindow
        {
            get
            {
                return currentWindow;
            }
            set
            {
                currentWindow = value;
                OnPropertyChanged("CurrentWindow");
            }
        }

        public DataView HandlesView
        {
            get
            {
                return handlesView;
            }
            set
            {
                handlesView = value;
                OnPropertyChanged("HandlesView");
            }
        }

        public string ChildWindowsVisibility
        {
            get
            {
                return childWindowsVisibility;
            }
            set
            {
                childWindowsVisibility = value;
                OnPropertyChanged("ChildWindowsVisibility");
            }
        }
    }
}
