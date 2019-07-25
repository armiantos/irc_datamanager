using irc_core.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.Dialogs
{
    public class AddDatabaseSource : BaseDataSourceDialog
    {
        private List<string> supportedDbs;
        private string selectedDb;
        private string host;
        private string username;

        private ICommand createDatabaseSourceCommand;

        public AddDatabaseSource()
        {
            supportedDbs = new List<string>
            {
                "Cassandra",
                "mongoDB",
                "MySQL",
                "SQL Server"
            };
            SelectedDb = SupportedDbs[0];
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

        public ICommand CreateDatabaseSourceCommand
        {
            get
            {
                if (createDatabaseSourceCommand == null)
                {
                    createDatabaseSourceCommand = new CommandWrapper(param =>
                    {
                        CreateDatabaseSource(SelectedDb, 
                            Host, 
                            Username, 
                            ((PasswordBox)param).Password);
                        Close();
                    });
                }
                return createDatabaseSourceCommand;
            }
        }

        private void CreateDatabaseSource(string type, string host, string username, string password)
        {
            DatabaseSource dbSource = new DatabaseSource(type, host, username, password) { Label = type + " @ " + host };
            // don't know if user credentials are correct
            NotifyNewDataSource(dbSource);
        }
    }
}
