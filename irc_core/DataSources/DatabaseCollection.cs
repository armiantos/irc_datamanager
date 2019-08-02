using irc_core.Dialogs;
using irc_core.Models;
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
                    {
                        ExportData(null);
                    });
                return exportDataCommand;
            }
        }

        #region methods 

        public async void AddDataView(string type, List<string> tags)
        {
            if (string.IsNullOrEmpty(type))
            {
                DataTable listData = await Task.Run(() => ListData());
                DataModelConfigDialog DataModelConfigDialog = new DataModelConfigDialog(listData);
                Dialog.Show(DataModelConfigDialog, DialogClosingEventHandler);
            }
            else
            {
                DataModel dataModel = await GetDataModel(type, tags);
                DataModels.Add(dataModel);
            }
        }

        private void DialogClosingEventHandler(object sender, ClosingEventArgs args)
        {
            if (args.Parameter != null && (bool)args.Parameter == true)
            {
                DataModelConfigDialog DataModelConfigDialog = (DataModelConfigDialog)args.Content;
                AddDataView(DataModelConfigDialog.GetSelectedViewType(), DataModelConfigDialog.GetIncluded());
            }
        }

        private void CloseDataView(object dataModel)
        {
            DataModels.Remove(DataModels.FirstOrDefault(o =>
                o == (DataModel)dataModel));
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

        /// <summary>
        /// Fills the file with data according to the filter arguments given.
        /// </summary>
        /// <param name="fs">Filestream to write to</param>
        /// <param name="filterArgs"></param>
        protected abstract void ExportData(object filterArgs);

        #endregion
    }
}
