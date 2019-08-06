using irc_core.Dialogs;
using irc_core.Models;
using irc_core.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.DataSources
{
    public abstract class DatabaseCollection : DataSource
    {
        public ObservableCollection<DataModel> DataModels { get; set; }

        private ICommand addDataViewCommand;

        private ICommand closeDataViewCommand;

        private ICommand exportDataCommand;

        public DatabaseCollection()
        {
            DataModels = new ObservableCollection<DataModel>();

            // runs in separate thread to update in the background 
            new Thread(() =>
            {
                while (true)
                {
                    foreach (DataModel model in DataModels)
                    {
                        if (model.IsLive)
                        {
                            Update(model);
                        }
                    }
                    Thread.Sleep(1000);
                }
            }) { IsBackground = true }.Start();
        }

        public ICommand AddDataViewCommand
        {
            get
            {
                if (addDataViewCommand == null)
                    addDataViewCommand = new RelayCommand(param =>
                    AddDataView(null, null));
                return addDataViewCommand;
            }
        }

        public ICommand CloseDataViewCommand
        {
            get
            {
                if (closeDataViewCommand == null)
                    closeDataViewCommand = new RelayCommand(param =>
                    CloseDataView(param));
                return closeDataViewCommand;
            }
        }

        public ICommand ExportDataCommand
        {
            get
            {
                if (exportDataCommand == null)
                    exportDataCommand = new RelayCommand(param =>
                    ExportData(null));
                return exportDataCommand;
            }
        }


        #region methods 

        public async void AddDataView(string type, List<string> tags)
        {
            if (string.IsNullOrEmpty(type))
            {
                DataTable listData = await Task.Run(() => ListData());
                AddDataViewDialog AddDataViewDialogView = new AddDataViewDialog(listData);
                Dialog.Show(AddDataViewDialogView, DialogClosingEventHandler);
            }
            else
            {
                DataModel dataModel = await GetDataModel(type, tags);
                DataModels.Add(dataModel);
            }
        }

        private void DialogClosingEventHandler(object sender, ClosingEventArgs args)
        {
            if (args.Parameter != null)
            {
                if ((AddDataViewDialog.Action)args.Parameter == AddDataViewDialog.Action.AddDataView)
                {
                    AddDataViewDialog addDataViewDialog = (AddDataViewDialog)args.Content;
                    AddDataView(addDataViewDialog.GetSelectedViewType(), addDataViewDialog.GetIncluded());
                }
                if ((ExportDataDialog.Action)args.Parameter == ExportDataDialog.Action.ExportData)
                {
                    ExportDataDialog exportDataDialog = (ExportDataDialog)args.Content;
                    ExportData(exportDataDialog.GetIncluded());
                }
            }
        }

        private void CloseDataView(object dataModel)
        {
            DataModels.Remove(DataModels.FirstOrDefault(o =>
                o == (DataModel)dataModel));
        }

        private async void ExportData(List<string> tags)
        {
            if (tags == null)
            {
                DataTable listData = await Task.Run(() => ListData());
                ExportDataDialog exportDataDialog = new ExportDataDialog(listData);
                Dialog.Show(exportDataDialog, DialogClosingEventHandler);
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV File |*.csv";
                if (saveFileDialog.ShowDialog() == true)
                {
                    MainViewModel.MessageQueue.Enqueue("Saving file");
                    await SaveToFile(tags, saveFileDialog.FileName);
                    MainViewModel.MessageQueue.Enqueue("Saved file! :)");
                }
            }
        }

        #endregion

        #region abstract methods

        /// <summary>
        /// Returns a table containing 3 columns: Included (bool), Tag (string), Type (type as string).
        /// To be displayed in a window to allow users to select a subset of data from.
        /// 
        /// e.g. :
        /// +---------+---------------------------------------------+---------------+
        /// | Include |                     Tag                     |     Type      |
        /// +---------+---------------------------------------------+---------------+
        /// | false   | FluidMech_BNOT_SecondaryTankEmpty_EnableIn  | System.Int32  |
        /// | false   | FluidMech_BNOT_SecondaryTankEmpty_EnableOut | System.Int32  |
        /// | false   | FluidMech_SuctionPIDECveuOutput             | System.Double |
        /// +---------+---------------------------------------------+---------------+
        /// 
        /// The type column can be filled with GetType() method from objects.
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Task<DataTable> ListData();

        /// <summary>
        /// Returns the appropriate data model given the type (e.g. plot or table view) of the 
        /// corresponding tags.
        /// </summary>
        /// <param name="type">string containing information about view type</param>
        /// <param name="tags">list of tags or columns to be retrieved from database</param>
        /// <returns></returns>
        public abstract Task<DataModel> GetDataModel(string type, List<string> tags);

        /// <summary>
        /// Updates dataviews with the latest data.
        /// </summary>
        /// <param name="model">data view to be updated</param>
        /// <returns></returns>
        protected abstract Task Update(DataModel model);

        protected abstract Task SaveToFile(List<string> tags, string path);

        #endregion
    }
}
