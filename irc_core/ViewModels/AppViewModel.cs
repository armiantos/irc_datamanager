using irc_core.DataSources;
using irc_core.Dialogs;
using System;
using System.Collections.Generic;
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
            Console.WriteLine($"DataSourceEvent sender: {sender}");
            if (args.Type == DataSourceEventArgs.EventType.Database)
            {
                var itemList = (List<string>)args.Message;
                CurrentDialogHost = new ListDialog(sender, itemList);
                CurrentDialogHost.Show();
                ((ListDialog)CurrentDialogHost).OnSelectEvent += ListDialogEventHandler;
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
            }
        }

        #endregion
    }
}
