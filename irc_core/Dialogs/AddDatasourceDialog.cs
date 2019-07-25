using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.Dialogs
{
    public class AddDatasourceDialog : ObservableObject
    {
        private string host;
        private string username;
        private string selectedDb;

        private List<string> supportedDbs;

        public AddDatasourceDialog()
        {
            SupportedDbs = new List<string>
                {
                    "Cassandra",
                    "mongoDB",
                    "MySQL",
                };
            SelectedDb = supportedDbs[0];
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

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public string SelectedDb
        {
            get
            {
                return selectedDb;
            }
            set
            {
                selectedDb = value;
                OnPropertyChanged("SelectedDb");
            }
        }

        public List<string> SupportedDbs
        {
            get
            {
                return supportedDbs;
            }
            set
            {
                supportedDbs = value;
                OnPropertyChanged("SupportedDbs");
            }
        }
    }
}
