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

        public ICommand CloseDialogCommand
        {
            get
            {
                if (closeDialogCommand == null)
                    closeDialogCommand = new CommandWrapper(param =>
                    CloseDialog(param));
                return closeDialogCommand;
            }
        }

        public AppViewModel()
        {
            PlotViewModel = new PlotViewModel();

            DataSources = new ObservableCollection<IDataSource>();

            addDatasourceDialog = new AddDatasourceDialog();

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
                    OpenDialog(param));
                return openDialogCommand;
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
        }

        private void AddNewDataSource()
        {
            DatabaseSource dbSource = new DatabaseSource();
            DataSources.Add(dbSource);
            dbSource.Name = RandomString(7);
        }

        private void OpenDialog(object param)
        {
            IsDialogHostOpen = "True";
        }

        private void CloseDialog(object param)
        {
            IsDialogHostOpen = "False";
        }

        /// <summary>
        /// temporary generate random name
        /// </summary>
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvxwz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
