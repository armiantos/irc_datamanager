using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.Dialogs
{
    public class TableDialog : Dialog
    {
        private DataView dataView;

        private object originalSender;

        private string searchField;

        private ICommand addTableViewCommand;

        public delegate void OkEventHandler(object sender, TableDialogEventArgs e);

        public event OkEventHandler OnOkEvent;

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

        public string SearchField
        {
            get
            {
                return searchField;
            }
            set
            {
                searchField = value;
                OnPropertyChanged("SearchField");
            }
        }

        public ICommand AddTableViewCommand
        {
            get
            {
                if (addTableViewCommand == null)
                    addTableViewCommand = new CommandWrapper(param =>
                    AddTableView());
                return addTableViewCommand;
            }
        }


        public TableDialog(object originalSender)
        {
            this.originalSender = originalSender;
        }

        public TableDialog(object originalSender, DataTable table)
        {
            this.originalSender = originalSender;
            DataView = table.AsDataView();
        }

        private void AddTableView()
        {
            OnOkEvent(originalSender, new TableDialogEventArgs());
            Close();
        }

    }

    public class TableDialogEventArgs
    {
        // todo
    }
}
