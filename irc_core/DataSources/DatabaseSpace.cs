using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DataSources
{
    public abstract class DatabaseSpace : DataSource
    {
        private string label;

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

        public abstract Task<List<string>> ListCollections();
    }
}
