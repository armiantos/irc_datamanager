using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.Dialogs
{
    public class TableDialog : Dialog
    {
        private DataView dataView;

        public DataView DataView
        {
            get
            {
                return dataView;
            }
            set
            {
                dataView = value;
                OnPropertyChanged("DataView");
            }
        }

        private object originalSender;

        public TableDialog(object originalSender)
        {
            this.originalSender = originalSender;
        }

        public TableDialog(object originalSender, DataTable table)
        {
            this.originalSender = originalSender;
            DataView = table.AsDataView();
        }

        public delegate void OkEventHandler(object sender, TableDialogEventArgs e);

        public event OkEventHandler OnSelectEvent;
    }

    public class TableDialogEventArgs
    {

    }
}
