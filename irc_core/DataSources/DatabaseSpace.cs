using irc_core.Dialogs;
using irc_core.Models;
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
                listDialog.Show(DialogClosingEventHandler);
            }
            else
            {
                DatabaseCollection dbCol = GetCollection(collectionName);
                Collections.Add(dbCol);
            }
        }

        private void DialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null && eventArgs.Parameter is string)
            {
                string param = (string)eventArgs.Parameter;
                AddCollection(param);
            }
        }

        public abstract Task<List<string>> ListCollections();

        public abstract DatabaseCollection GetCollection(string name);
    }
}
