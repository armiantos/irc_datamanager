using irc_core.DatabaseLibrary;
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
                        AddSpace();
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

        private async void AddSpace()
        {
            var spaces = await client.ListDatabases();
            NotifyDataSourceEvent(this, new DataSourceEventArgs(DataSourceEventArgs.EventType.Database,
                DataSourceEventArgs.MessageType.SpaceList, spaces));
        }

        public void AddSpace(string name)
        {
            Spaces.Add(client.GetDatabase(name));
        }


    }
}
