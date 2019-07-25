using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.DataSources
{
    public class DatabaseSpace : DataSource
    {
        public ObservableCollection<DatabaseSpace> Collections { get; set; }

        public DatabaseSpace()
        {
            Collections = new ObservableCollection<DatabaseSpace>();
        }
    }
}
