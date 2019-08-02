﻿using irc_core.HelperClasses;
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
    public class DataModelConfigDialog : Dialog
    {
        private DataView dataView;

        private string searchField;

        private ICommand addDataViewCommand;

        private ICommand searchTextboxCommand;

        private ICommand saveDataCommand;

        public enum Action { AddDataView, SaveData}       

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

        /// <summary>
        /// Triggered when the add button is clicked on the dialog.
        /// </summary>
        public ICommand AddDataViewCommand
        {
            get
            {
                if (addDataViewCommand == null)
                    addDataViewCommand = new RelayCommand(param =>
                        CloseDataModelConfigDialog(Action.AddDataView));
                return addDataViewCommand;
            }
        }

        public ICommand SaveDataCommand
        {
            get
            {
                if (saveDataCommand == null)
                    saveDataCommand = new RelayCommand(param =>
                        CloseDataModelConfigDialog(Action.SaveData));
                return saveDataCommand;
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
        public DataModelConfigDialog()
        {
            SupportedViews = new ObservableCollection<StringBool>
            {
                new StringBool{Label = "Plot", Boolean = false},
                new StringBool{Label = "Table", Boolean = false}
            };
            included = new List<string>();
        }

        public DataModelConfigDialog( DataTable table) : this()
        {
            DataView = table.AsDataView();
        }

        private void CloseDataModelConfigDialog(Action action)
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
