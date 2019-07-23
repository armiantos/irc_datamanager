using irc_datamanager.DataSourceWrappers;
using irc_datamanager.HelperClasses;
using irc_datamanager.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace irc_datamanager.ViewModels
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
