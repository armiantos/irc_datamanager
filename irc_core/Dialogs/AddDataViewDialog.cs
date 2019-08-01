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
        private ICommand searchTextboxCommand;
        

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

        public ICommand SearchTextboxCommand
        {
            get
            {
                if (searchTextboxCommand == null)
                {
                    searchTextboxCommand = new CommandWrapper(param =>
                    {
                        if (dataView != null)
                        {
                            DataTable dt = dataView.Table;
                            DataView = dt.AsEnumerable().Where(row => row["Tag"].ToString()
                                .ToLower().Contains(searchField))
                                .AsDataView();
                        }
                    });
                }
                return searchTextboxCommand;
            }
        }

        public List<string> GetIncluded()
        {
            return included;
        }

        public string GetSelectedType()
        {
            string selectedType = SupportedViews.FirstOrDefault(entry => entry.Boolean == true).Label;
            if (!string.IsNullOrEmpty(selectedType))
            {
                return selectedType;
            }
            throw new InvalidOperationException();
        }

        public ObservableCollection<StringBool> SupportedViews { get; set; }
        #endregion

        #region methods
        public AddDataViewDialog()
        {
            SupportedViews = new ObservableCollection<StringBool>
            {
                new StringBool{Label = "Plot", Boolean = false},
                new StringBool{Label = "Table", Boolean = false}
            };
            included = new List<string>();
        }

        public AddDataViewDialog( DataTable table) : this()
        {
            DataView = table.AsDataView();
        }

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
