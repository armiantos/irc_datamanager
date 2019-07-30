using irc_core.Dialogs;
using irc_core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.DataSources
{
    public abstract class DatabaseCollection : DataSource
    {
        private string label;

        public ObservableCollection<DataModel> DataViews { get; set; }

        private ICommand addDataViewCommand;

        private ICommand closeDataViewCommand;

        public DatabaseCollection()
        {
            DataViews = new ObservableCollection<DataModel>();

            new Thread(() =>
            {
                while (true)
                {
                    foreach (var dataView in DataViews)
                    {
                        Update(dataView);
                    }
                    Thread.Sleep(1000);
                }
            }) { IsBackground = true }.Start();
        }

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
                OnPropertyChanged("Name");
            }
        }

        public ICommand AddDataViewCommand
        {
            get
            {
                if (addDataViewCommand == null)
                    addDataViewCommand = new CommandWrapper(param =>
                    AddDataView(null, null));
                return addDataViewCommand;
            }
        }

        public ICommand CloseDataViewCommand
        {
            get
            {
                if (closeDataViewCommand == null)
                    closeDataViewCommand = new CommandWrapper(param =>
                    CloseDataView(param));
                return closeDataViewCommand;
            }
        }

        public async void AddDataView(string type, List<string> tags)
        {
            if (string.IsNullOrEmpty(type))
            {
                DataTable listData = await Task.Run(() => ListData());
                AddDataViewDialog addDataViewDialog = new AddDataViewDialog(listData);
                Dialog.Show(addDataViewDialog, DialogClosingEventHandler);
            }
            else
            {
                DataModel dataModel = await GetDataModel(type, tags);
                DataViews.Add(dataModel);
            }
        }

        private void DialogClosingEventHandler(object sender, ClosingEventArgs args)
        {
            if (args.Parameter != null && (bool)args.Parameter == true)
            {
                Console.WriteLine(args.Content);
                AddDataViewDialog addDataViewDialog = (AddDataViewDialog)args.Content;
                AddDataView(addDataViewDialog.GetSelectedType(), addDataViewDialog.GetIncluded());
            }
        }

        private void CloseDataView(object dataModel)
        {
            DataViews.Remove(DataViews.FirstOrDefault(o =>
                o == (DataModel)dataModel));
        }

        public abstract Task<DataTable> ListData();

        public abstract Task<DataModel> GetDataModel(string type, List<string> tags);

        protected abstract Task Update(DataModel model);
    }

}
