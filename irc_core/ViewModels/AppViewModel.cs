using irc_core.DataSources;
using irc_core.Dialogs;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
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
                    AddDataSource());
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

        private void AddDataSource()
        {
            CurrentDialogHost = new AddDataSourceDialog();
            CurrentDialogHost.Show();
            ((AddDataSourceDialog)CurrentDialogHost).OnNewDataSource += NewDataSourceHandler;
        }

        private void NewDataSourceHandler(DataSource newDataSource)
        {
            DataSources.Add(newDataSource);
            newDataSource.OnDataSourceEvent += DataSourceEventHandler; ;
        }

        private void DataSourceEventHandler(object sender, DataSourceEventArgs args)
        {
            if (args.Type == DataSourceEventArgs.EventType.Database)
            {
                var itemList = (List<string>)args.Message;
                CurrentDialogHost = new ListDialog(sender, itemList);
                CurrentDialogHost.Show();
                ((ListDialog)CurrentDialogHost).OnSelectEvent += ListDialogEventHandler;
            }
            else if (args.Type == DataSourceEventArgs.EventType.Views)
            {
                if (args.MsgType == DataSourceEventArgs.MessageType.DataTable)
                {
                    CurrentDialogHost = new AddDataViewDialog(sender, (DataTable)args.Message);
                    CurrentDialogHost.Show(TableDialogClosingHandler);
                }
            }
        }


        public void TableDialogClosingHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Session.Content is AddDataViewDialog)
            {
                DatabaseCollection originalSender = (DatabaseCollection)((AddDataViewDialog)eventArgs.Session.Content).OriginalSender;
                AddDataViewDialog dialog = (AddDataViewDialog)eventArgs.Session.Content;
                string type = dialog.SupportedViews.FirstOrDefault(obj => obj.Boolean == true).Label;
                originalSender.AddDataView(type, dialog.GetIncluded());
            }
        }

        private void ListDialogEventHandler(object sender, ListDialogEventArgs e)
        {
            // handle select event
            if (e.Type == ListDialogEventArgs.EventType.Select)
            {
                if (sender is DatabaseSource)
                {
                    var originalSender = (DatabaseSource)sender;
                    originalSender.AddSpace((string)e.Message);
                }
                else if (sender is DatabaseSpace)
                {
                    var originalSender = (DatabaseSpace)sender;
                    originalSender.AddCollection((string)e.Message);
                }
                else if (sender is DatabaseCollection)
                {
                    var originalSender = (DatabaseCollection)sender;
                    originalSender.NotifyViewType((string)e.Message);
                }
            }
        }

        #endregion
    }
}
