using irc_core.DataSources;
using irc_core.Views;
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
        private BaseDataSourceDialog currentDataSource;
        #endregion

        public AddDataSourceDialog() : base()
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
                CurrentDataSource = DataSourceFactory.CreateDataSource(value, this);
                OnPropertyChanged("SelectedType");
            }
        }

        public BaseDataSourceDialog CurrentDataSource
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

        #endregion
    }

    public static class DataSourceFactory
    {
        public static BaseDataSourceDialog CreateDataSource(string type, AddDataSourceDialog mainDialog)
        {
            if (type == "Databases")
            {
                return new AddDatabaseSource(mainDialog);
            }
            throw new NotImplementedException();
        }
    }
}
