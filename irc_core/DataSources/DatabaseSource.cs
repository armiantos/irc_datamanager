using irc_core.DatabaseLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DataSources
{
    public class DatabaseSource : DataSource
    {
        private IDatabase client;

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

        public ObservableCollection<DatabaseSpace> Spaces { get; set; }

        public DatabaseSource(string type, string host, string username, string password)
        {
            Spaces = new ObservableCollection<DatabaseSpace>();
            IDatabase client = DbFactory.CreateDatabase(type);
        }
    }


}
