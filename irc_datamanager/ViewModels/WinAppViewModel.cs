using irc_connector.DataSourceWrappers;
using irc_connector.HelperClasses;
using irc_connector.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace irc_connector.ViewModels
{
    class WinAppViewModel : ViewModel
    {
        public override string ViewModelName
        {
            get
            {
                return "Windows Application";
            }
        }

        public WinAppModel Model { get; } 

        public WinAppViewModel()
        {
            Model = new WinAppModel();
            Model.ChildWindowsVisibility = "Collapsed";
        }
        
    }
}
