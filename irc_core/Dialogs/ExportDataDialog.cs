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
    public class ExportDataDialog : Dialog
    {
        private DataView dataView;

        private string searchField;

        private ICommand exportDataCommand;

        private ICommand searchTextboxCommand;

        private DateTime initialDate;
        private DateTime finalDate;
        private DateTime initialTime;
        private DateTime finalTime;
        
        public enum Action { AddDataView, ExportData}       

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

        public DateTime InitialDate
        {
            get
            {
                if (initialDate == null)
                    initialDate = DateTime.UtcNow;
                return initialDate;
            }
            set
            {
                initialDate = value;
                Console.WriteLine($"initialdate:{value}");
                OnPropertyChanged("InitialDate");
            }
        }

        public DateTime InitialTime
        {
            get
            {
                if (initialTime == null)
                    initialTime = DateTime.UtcNow;
                return initialTime;
            }
            set
            {
                initialTime = value;
                Console.WriteLine($"initialtime:{value}");
                OnPropertyChanged("InitialTime");
            }
        }

        public DateTime FinalTime
        {
            get
            {
                if (finalTime == null)
                    finalTime = DateTime.UtcNow;
                return finalTime;
            }
            set
            {
                finalTime = value;
                Console.WriteLine($"finaltime:{value}");
                OnPropertyChanged("FinalTime");
            }
        }

        public DateTime FinalDate
        {
            get
            {
                if (finalDate == null)
                    finalDate = DateTime.UtcNow;
                return finalDate;
            }
            set
            {
                finalDate = value;
                Console.WriteLine($"finaldate:{value}");
                OnPropertyChanged("FinalDate");
            }
        }

        /// <summary>
        /// Triggered when the add button is clicked on the dialog.
        /// </summary>
        public ICommand ExportDataCommand
        {
            get
            {
                if (exportDataCommand == null)
                    exportDataCommand = new RelayCommand(param =>
                        CloseExportDataDialogView(Action.ExportData));
                return exportDataCommand;
            }
        }


        /// <summary>
        /// Triggered when enter key is pressed in search box. Updates data view in
        /// datagrid to only show rows that contain the given substring.
        /// </summary>
        public ICommand SearchTextboxCommand
        {
            get
            {
                if (searchTextboxCommand == null)
                {
                    searchTextboxCommand = new RelayCommand(param =>
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

        public Tuple<DateTime, DateTime> GetTimeRange()
        {
            Console.WriteLine(initialDate);
            Console.WriteLine(initialTime);
            Console.WriteLine(finalDate);
            Console.WriteLine(finalTime);
            return new Tuple<DateTime, DateTime>(initialDate, finalDate);
        }

        public string GetSelectedViewType()
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
        public ExportDataDialog()
        {
            SupportedViews = new ObservableCollection<StringBool>
            {
                new StringBool{Label = "Plot", Boolean = false},
                new StringBool{Label = "Table", Boolean = false}
            };
            included = new List<string>();
        }

        public ExportDataDialog( DataTable table) : this()
        {
            DataView = table.AsDataView();
        }

        private void CloseExportDataDialogView(Action action)
        {
            var o = dataView.Table.AsEnumerable().Where(p => (bool)p["Include"]);

            foreach (DataRow r in o)
            {
                included.Add((string)r["Tag"]);
            }
            Close(action);
        }
        #endregion
    }
}
