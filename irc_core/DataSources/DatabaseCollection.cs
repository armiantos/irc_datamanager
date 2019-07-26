﻿using irc_core.Dialogs;
using irc_core.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
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
                    AddDataView());
                return addDataViewCommand;
            }
        }

        public ICommand CloseDataViewCommand
        {
            get
            {
                if (closeDataViewCommand == null)
                    closeDataViewCommand = new CommandWrapper(param =>
                    RemoveDataView(param));
                return closeDataViewCommand;
            }
        }

        private async void AddDataView()
        {
            DataTable listData = await ListData();
            NotifyDataSourceEvent(this, new DataSourceEventArgs(DataSourceEventArgs.EventType.Views,
                DataSourceEventArgs.MessageType.DataTable, listData));
        }

        public void NotifyViewType(string type)
        {
            throw new NotImplementedException();
        }

        public void NotifyTableDialogEvent(TableDialogEventArgs e)
        {
            Console.WriteLine(e);
        }

        private void AddDataView(string type, List<string> tags)
        {
            Console.WriteLine(type);
            foreach (string tag in tags)
            {
                Console.WriteLine(tag);
            }
        }

        private void RemoveDataView(object dataView)
        {
            throw new NotImplementedException();
        }

        public abstract Task<DataTable> ListData();

        public abstract Task<DataModel> GetDataModel(string type, List<string> labels);
    }
}
