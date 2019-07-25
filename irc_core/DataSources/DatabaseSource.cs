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
            Console.WriteLine("listing spaces");
            var spaces = await client.ListDatabases();
            foreach (string space in spaces)
            {
                Console.WriteLine(space);
            }
            NotifyDataSourceEvent(this, spaces);
        }


    }


}
