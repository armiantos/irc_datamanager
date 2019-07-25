using irc_core.DataSources;
using irc_core.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.ViewModels
{
    public class AppViewModel : ObservableObject
    {
        #region privates

        private PlotViewModel plotViewModel;

        private ICommand addDataSourceCommand;

        private Dialog currentDialogHost;

        public ObservableCollection<DataSource> DataSources { get; set; }

        #endregion
               
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

        public AppViewModel()
        {
            PlotViewModel = new PlotViewModel();

            DataSources = new ObservableCollection<DataSource>();
        }

        private void AddNewDataSource()
        {
            CurrentDialogHost = new AddDataSourceDialog();
            OpenDialog();
        }

        private void OpenDialog()
        {
            AddDataSourceDialog addDataSourceDialog = new AddDataSourceDialog();
            CurrentDialogHost = addDataSourceDialog;
            CurrentDialogHost.Show();
        }

        #endregion
    }
}
