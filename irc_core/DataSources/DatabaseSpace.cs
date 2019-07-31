using irc_core.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;
using System.Linq;

namespace irc_core.DataSources
{
    public abstract class DatabaseSpace : DataSource
    {
        private string label;

        private ICommand addCollectionCommand;

        public ObservableCollection<DatabaseCollection> Collections { get; set; }


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
            Collections = new ObservableCollection<DatabaseCollection>();
        }

        public ICommand AddCollectionCommand
        {
            get
            {
                if (addCollectionCommand == null)
                    addCollectionCommand = new CommandWrapper(param =>
                        AddCollection(null));
                return addCollectionCommand;
            }
        }

        private async void AddCollection(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
            {
                var collections = await Task.Run(() => ListCollections());
                ListDialog listDialog = new ListDialog(this, collections);
                Dialog.Show(listDialog, DialogClosingEventHandler);
            }
            else
            {
                if (Collections.FirstOrDefault(collection => collection.Label == collectionName) == null)
                {
                    Collections.Add(GetCollection(collectionName));
                }
            }
        }

        private void DialogClosingEventHandler(object sender, ClosingEventArgs args)
        {
            if (args.Parameter != null && args.Parameter is string)
            {
                string param = (string)args.Parameter;
                AddCollection(param);
            }
        }

        public abstract Task<List<string>> ListCollections();

        public abstract DatabaseCollection GetCollection(string name);
    }
}
