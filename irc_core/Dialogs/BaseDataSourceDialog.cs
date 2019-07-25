using irc_core.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.Dialogs
{
    abstract public class BaseDataSourceDialog : Dialog
    {
        public delegate void NewDataSourceEventHandler(DataSource newDataSource);

        public event NewDataSourceEventHandler OnNewDataSource;

        public void NotifyNewDataSource(DataSource newDataSource)
        {
            Close();
            OnNewDataSource(newDataSource);
        }
    }
}
