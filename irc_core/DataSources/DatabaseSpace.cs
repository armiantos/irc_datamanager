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
    public abstract class DatabaseSpace : DataSource
    {
        private string label;

        private ICommand addCollectionCommand;

        public ObservableCollection<DatabaseSpace> Collections { get; set; }

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

        public DatabaseSpace()
        {
            Collections = new ObservableCollection<DatabaseSpace>();
        }

        public ICommand AddCollectionCommand
        {
            get
            {
                if (addCollectionCommand == null)
                    addCollectionCommand = new CommandWrapper(param =>
                        AddCollection());
                return addCollectionCommand;
            }
        }

        public abstract Task<List<string>> ListCollections();

        private async void AddCollection()
        {
            var collections = await ListCollections();
            NotifyDataSourceEvent(this, new DataSourceEventArgs(DataSourceEventArgs.EventType.Database,
                DataSourceEventArgs.MessageType.CollectionList, collections));
        }

        public void AddCollection(string name)
        {
            Console.WriteLine(name);
        }
    }
}
