using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DataSources
{
    class DatabaseSource : DataSource
    {
        private string label;

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

        public ObservableCollection<DatabaseCollection> Collections { get; set; }

        public DatabaseSource()
        {
            Collections = new ObservableCollection<DatabaseCollection>();
        }
    }
}
