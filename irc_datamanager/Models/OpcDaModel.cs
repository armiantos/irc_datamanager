using irc_connector.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_connector.Models
{
    public class OpcDaModel : ObservableObject
    {
        private DataView itemsView;

        private string host;
        private string itemsViewVisibility;
        private string opcServersVisibility;
        private string currentOpcServer;
        private string searchField;
        private List<string> opcServers;

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

        public List<string> OpcServers
        {
            get
            {
                return opcServers;
            }
            set
            {
                opcServers = value;
                OnPropertyChanged("OpcServers");
            }
        }

        public DataView ItemsView
        {
            get
            {
                return itemsView;
            }
            set
            {
                if (value.Count > 0)
                {
                    itemsView = value;
                    OnPropertyChanged("ItemsView");
                }
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

        public string CurrentOpcServer
        {
            get
            {
                return currentOpcServer;
            }
            set
            {
                currentOpcServer = value;
                OnPropertyChanged("CurrentOpcServer");
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
    }
}
