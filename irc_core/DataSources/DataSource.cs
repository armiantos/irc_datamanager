using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSharedLibrary;

namespace irc_core.DataSources
{
    public abstract class DataSource : ObservableObject
    {
        public delegate void DataSourceEventHandler(DataSource sender, object message);

        public event DataSourceEventHandler OnDataSourceEvent;

        public void NotifyDataSourceEvent(DataSource sender, object message)
        {
            OnDataSourceEvent(sender, message);
        }
    }
}
