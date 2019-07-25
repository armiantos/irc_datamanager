using irc_core.DataSources;
using irc_core.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.ViewModels
{
    public class AppViewModel : ObservableObject
    {
        #region privates

        private PlotViewModel plotViewModel;

        private ICommand addDataSourceCommand;

        private ICommand openDialogCommand;

        private ICommand closeDialogCommand;

        private Dialog currentDialogHost;

        public ObservableCollection<DataSource> DataSources { get; set; }

        #endregion



        public AppViewModel()
        {
            PlotViewModel = new PlotViewModel();

            DataSources = new ObservableCollection<DataSource>();
        }

        #region public getters, setters

        public PlotViewModel PlotViewModel
        {
            get
            {
                return plotViewModel;
            }
            set
            {
                plotViewModel = value;
                OnPropertyChanged("PlotViewModel");
            }
        }

        public ICommand AddDataSourceCommand
        {
            get
            {
                if (addDataSourceCommand == null)
                    addDataSourceCommand = new CommandWrapper(param =>
                    AddNewDataSource());
                return addDataSourceCommand;
            }
        }

        public ICommand OpenDialogCommand
        {
            get
            {
                if (openDialogCommand == null)
                    openDialogCommand = new CommandWrapper(param =>
                    OpenDialog());
                return openDialogCommand;
            }
        }

        public ICommand CloseDialogCommand
        {
            get
            {
                if (closeDialogCommand == null)
                    closeDialogCommand = new CommandWrapper(param =>
                    CloseDialog(((PasswordBox)param).Password));
                return closeDialogCommand;
            }
        }


        public Dialog CurrentDialogHost
        {
            get
            {
                return currentDialogHost;
            }
            set
            {
                currentDialogHost = value;
                OnPropertyChanged("CurrentDialogHost");
            }
        }
        #endregion

        #region methods

        private void AddNewDataSource()
        {
            CurrentDialogHost = new AddDataSourceDialog();
            OpenDialog();
        }

        private async void OpenDialog()
        {
            AddDataSourceDialog addDataSourceDialog = new AddDataSourceDialog();
            CurrentDialogHost = addDataSourceDialog;
            CurrentDialogHost.Show();
        }

        private void CloseDialog(string password)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
