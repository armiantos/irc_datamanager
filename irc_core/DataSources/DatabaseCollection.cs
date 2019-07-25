using irc_core.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.DataSources
{
    class DatabaseCollection : DataSource
    {
        private string label;

        public ObservableCollection<PlotModel> Plots { get; set; }

        private ICommand addDataViewCommand;

        private ICommand closeDataViewCommand;

        public DatabaseCollection()
        {
            Plots = new ObservableCollection<PlotModel>();
            Label = RandomString(10);
            // handle new database connection
        }

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
                OnPropertyChanged("Name");
            }
        }

        public ICommand AddDataViewCommand
        {
            get
            {
                if (addDataViewCommand == null)
                    addDataViewCommand = new CommandWrapper(param =>
                    AddNewDataView());
                return addDataViewCommand;
            }
        }

        public ICommand CloseDataViewCommand
        {
            get
            {
                if (closeDataViewCommand == null)
                    closeDataViewCommand = new CommandWrapper(param =>
                    RemoveDataView(param));
                return closeDataViewCommand;
            }
        }

        private void AddNewDataView()
        {
            PlotModel mdl = new PlotModel(RandomString(5));
            for(int i = 0; i < random.Next(3) + 1; i++)
            {
                LineSeries line = new LineSeries();
                ChartValues<int> values = new ChartValues<int>();
                for (int j = 0; j < 20; j++)
                {
                    values.Add(random.Next(100));
                }
                line.Values = values;
                mdl.Series.Add(line);
            }
            Plots.Add(mdl);
        }

        private void RemoveDataView(object dataView)
        {
            Plots.Remove((PlotModel)dataView);
        }


        /// <summary>
        /// temporary - generate random string
        /// </summary>
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
