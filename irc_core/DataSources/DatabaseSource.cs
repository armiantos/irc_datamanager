using irc_core.DatabaseLibrary;
using irc_core.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.DataSources
{
    public class DatabaseSource : DataSource
    {
        private IDatabase client;

        private string label;

        private ICommand addSpaceCommand;

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
                OnPropertyChanged("Label");
            }
        }

        public ObservableCollection<DatabaseSpace> Spaces { get; set; }

        public ICommand AddSpaceCommand
        {
            get
            {
                if (addSpaceCommand == null)
                    addSpaceCommand = new CommandWrapper(param =>
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
                listDialog.Show(DialogClosingEventHandler);
            }
            else
            {
                DatabaseSpace dbSpace = client.GetDatabase(name);
                Spaces.Add(dbSpace);
            }
        }

        private void DialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null && eventArgs.Parameter is string)
            {
                string param = (string)eventArgs.Parameter;
                AddSpace(param);
            }
        }
    }
}
