﻿using irc_core.DatabaseLibrary;
using irc_core.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.DataSources
{
    public class DatabaseSource : DataSource
    {
        private IDatabase client;

        private ICommand addSpaceCommand;

        public ObservableCollection<DatabaseSpace> Spaces { get; set; }

        public ICommand AddSpaceCommand
        {
            get
            {
                if (addSpaceCommand == null)
                    addSpaceCommand = new RelayCommand(param =>
                    {
                        AddSpace(null);
                    });
                return addSpaceCommand;
            }
        }

        public DatabaseSource(string type, string host, string username, string password)
        {
            Spaces = new ObservableCollection<DatabaseSpace>();
            client = DbFactory.CreateDatabase(type);
            client.Connect(host, username, password);
        }


        private async void AddSpace(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var spaces = await Task.Run(() => client.ListDatabases());
                ListDialog listDialog = new ListDialog(this, spaces);
                Dialog.Show(listDialog, DialogClosingEventHandler);
            }
            else
            {
                DatabaseSpace dbSpace = client.GetDatabase(name);
                Spaces.Add(dbSpace);
            }
        }

        private void DialogClosingEventHandler(object sender, ClosingEventArgs args)
        {
            if (args.Parameter != null && args.Parameter is string)
            {
                string param = (string)args.Parameter;
                AddSpace(param);
            }
        }

    }
}
