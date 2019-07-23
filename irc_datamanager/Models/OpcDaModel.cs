using irc_datamanager.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_datamanager.Models
{
    public class OpcDaModel : ObservableObject
    {
        private string host;
        private DataTable itemsTable;
        private DataView itemsView;
        private string itemsViewVisibility;
        private string opcServersVisibility;

        public OpcDaModel()
        {
            OpcServersVisibility = "Hidden";
            ItemsViewVisibility = "Hidden";
        }

        public string Host
        {
            get
            {
                return host;
            }
            set
            {
                host = value;
                OnPropertyChanged("Host");
            }
        }

        public ObservableCollection<string> OpcServers { get; set; }

        public DataView ItemsView
        {
            get
            {
                return itemsView;
            }
            set
            {
                itemsView = value;
                OnPropertyChanged("ItemsView");
            }
        }

        public DataTable ItemsTable
        {
            get
            {
                return itemsTable;
            }
            set
            {
                itemsTable = value;
                OnPropertyChanged("ItemTable");
            }
        }

        public string OpcServersVisibility
        {
            get
            {
                return opcServersVisibility;
            }
            set
            {
                opcServersVisibility = value;
                OnPropertyChanged("OpcServersVisibility");
            }
        }

        public string ItemsViewVisibility
        {
            get
            {
                return itemsViewVisibility;
            }
            set
            {
                itemsViewVisibility = value;
                OnPropertyChanged("ItemsViewVisibility");
            }
        }

    }
}
