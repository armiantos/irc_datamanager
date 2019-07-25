using irc_core.DataSources;
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
    public class AddDataSourceDialog : Dialog
    {
        #region privates
        private List<string> supportedTypes;
        private string selectedType;
        private object currentDataSource;
        private ICommand dataSourceOKCommand;
        #endregion

        public AddDataSourceDialog()
        {
            SupportedTypes = new List<string>
            {
                "Databases",
            };

            SelectedType = SupportedTypes[0];
        }

        #region public getters, setters
        public List<string> SupportedTypes
        {
            get
            {
                return supportedTypes;
            }
            set
            {
                supportedTypes = value;
                OnPropertyChanged("SupportedTypes");
            }
        }

        public string SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                selectedType = value;
                CurrentDataSource = DataSourceFactory.CreateDataSource(value);
                OnPropertyChanged("SelectedType");
            }
        }

        public object CurrentDataSource
        {
            get
            {
                return currentDataSource;
            }
            set
            {
                currentDataSource = value;
                OnPropertyChanged("CurrentDataSource");
            }
        }

        public ICommand DataSourceOKCommand
        {
            get
            {
                if (dataSourceOKCommand == null)
                {
                    dataSourceOKCommand = new CommandWrapper(param =>
                    {
                        Close();
                        // build new data source in mainviewmodel
                    });
                }
                return dataSourceOKCommand;
            }
        }
        #endregion
    }

    public static class DataSourceFactory
    {
        public static object CreateDataSource(string type)
        {
            if (type == "Databases")
            {
                return new AddDatabaseSource();
            }
            throw new NotImplementedException();
        }
    }
}
