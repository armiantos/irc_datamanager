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
        private PlotViewModel plotViewModel;

        private ICommand addDataSourceCommand;

        private ICommand openDialogCommand;

        private ICommand closeDialogCommand;

        private string isDialogHostOpen;

        public ObservableCollection<IDataSource> DataSources { get; set; }

        private AddDatasourceDialog addDatasourceDialog;

        public AppViewModel()
        {
            PlotViewModel = new PlotViewModel();

            DataSources = new ObservableCollection<IDataSource>();

            isDialogHostOpen = "False";
        }

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


        public string IsDialogHostOpen
        {
            get
            {
                return isDialogHostOpen;
            }
            set
            {
                if (isDialogHostOpen != value)
                {
                    isDialogHostOpen = value;
                    OnPropertyChanged("IsDialogHostOpen");
                }
            }
        }

        public AddDatasourceDialog CurrentDialogHost
        {
            get
            {
                return addDatasourceDialog;
            }
            set
            {
                addDatasourceDialog = value;
                OnPropertyChanged("CurrentDialogHost");
            }
        }

        private void AddNewDataSource()
        {
            CurrentDialogHost = new AddDatasourceDialog();
            OpenDialog();
        }

        private void OpenDialog()
        {
            IsDialogHostOpen = "True";
        }

        private void CloseDialog(string password)
        {
            IsDialogHostOpen = "False";

            DatabaseSource dbSource = new DatabaseSource(addDatasourceDialog.SelectedDb,
                addDatasourceDialog.Host,
                addDatasourceDialog.Username,
                password);
            DataSources.Add(dbSource);
        }
    }
}
