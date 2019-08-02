using irc_connector.DataSourceWrappers;
using WpfSharedLibrary;
using irc_connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace irc_connector.ViewModels
{
    public class OpcDaViewModel : ViewModel
    {
        private ICommand listServersCommand;

        public override string ViewModelName
        {
            get
            {
                return "OPC DA";
            }
        }

        public OpcDaModel OpcDaModel { get; }


        public OpcDaViewModel()
        {
            OpcDaModel = new OpcDaModel();
            OpcDaModel.OpcServersVisibility = "Hidden";
            OpcDaModel.ItemsViewVisibility = "Hidden";

            OpcDaModel.PropertyChanged += ModelPropertyChangedHandler;
        }

        private void ModelPropertyChangedHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentOpcServer" && OpcDaModel.CurrentOpcServer != null)
            {
                OpcDaWrapper.GetInstance().Connect(OpcDaModel.CurrentOpcServer,
                    OpcDaModel.Host);
                DataTable itemsTable = new DataTable();
                itemsTable.Columns.Add("Include", typeof(bool));
                itemsTable.Columns.Add("Tag", typeof(string));
                foreach (string itemName in OpcDaWrapper.GetInstance().ListItems())
                {
                    DataRow row = itemsTable.NewRow();
                    row["Include"] = true;
                    row["Tag"] = itemName;
                    itemsTable.Rows.Add(row);
                }
                OpcDaModel.ItemsView = itemsTable.AsDataView();
                OpcDaModel.ItemsViewVisibility = "Visible";
            }
            else if (e.PropertyName == "OpcServers")
            {
                OpcDaModel.OpcServersVisibility = "Visible";
            }
            else if (e.PropertyName == "SearchField")
            {
                FilterItems(OpcDaModel.SearchField);
            }
        }

        public ICommand ListServersCommand
        {
            get
            {
                if (listServersCommand == null)
                {
                    listServersCommand = new RelayCommand(param =>
                    {
                        ConnectToHost();
                    });
                }
                return listServersCommand;
            }
        }
        
        private void ConnectToHost()
        {
            OpcDaModel.OpcServers = OpcDaWrapper.GetInstance().ListServers(OpcDaModel.Host);
        }

        private void FilterItems(string criterion)
        {
            DataTable dt = OpcDaModel.ItemsView.Table;
            OpcDaModel.ItemsView = dt.AsEnumerable().Where(row =>
                ((string)row["Tag"]).ToLower().Contains(criterion.ToLower()))
                .AsDataView();
        }
    }
}
