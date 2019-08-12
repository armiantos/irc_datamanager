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


        public DateTime InitialTime
        {
            get
            {
                return initialTime;
            }
            set
            {
                initialTime = value;
                OnPropertyChanged("InitialTime");
            }
        }

        public DateTime FinalTime
        {
            get
            {
                return finalTime;
            }
            set
            {
                finalTime = value;
                OnPropertyChanged("FinalTime");
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
        #endregion

        #region methods
        public List<string> GetIncluded()
        {
            return included;
        }

        public Tuple<DateTime, DateTime> GetTimeRange()
        {
            return new Tuple<DateTime, DateTime>(InitialTime, FinalTime);
        }

        public ExportDataDialog()
        {
            InitialTime = DateTime.UtcNow;
            FinalTime = DateTime.UtcNow;

            InitialTime -= new TimeSpan(0, 0, InitialTime.Second);

            FinalTime -= new TimeSpan(0, 0, FinalTime.Second);

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
