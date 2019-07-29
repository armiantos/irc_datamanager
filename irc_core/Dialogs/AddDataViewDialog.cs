using irc_core.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.Dialogs
{
    public class AddDataViewDialog : Dialog
    {
        private DataView dataView;

        private string searchField;

        private ICommand addTableViewCommand;

        private List<string> included;

        #region public getters and setters
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

        public List<string> GetIncluded()
        {
            return included;
        }

        public ObservableCollection<StringBool> SupportedViews { get; set; }
        #endregion

        #region methods
        public AddDataViewDialog(object originalSender)
        {
            OriginalSender = originalSender;
            SupportedViews = new ObservableCollection<StringBool>
            {
                new StringBool{Label = "Plot", Boolean = false},
                new StringBool{Label = "Table", Boolean = false}
            };
            included = new List<string>();
        }

        public AddDataViewDialog(object originalSender, DataTable table) : this(originalSender)
        {
            DataView = table.AsDataView();
        }

        public object OriginalSender { get; }

        private void AddTableView()
        {
            foreach (DataRow r in dataView.Table.Rows)
            {
                if ((bool)r["Include"] == true)
                {
                    included.Add((string)r["Tag"]);
                }
            }
            Close(true);
        }
        #endregion
    }
}
