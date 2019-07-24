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

namespace irc_core.ViewModels
{
    public class PlotViewModel
    {
        private ICommand addPlotViewCommand;

        public ObservableCollection<PlotModel> Plots { get; set; }

        public PlotViewModel()
        {
            Plots = new ObservableCollection<PlotModel>();
        }

        public ICommand AddPlotViewCommand
        {
            get
            {
                if (addPlotViewCommand == null)
                    addPlotViewCommand = new CommandWrapper(param =>
                    AddPlot(param));
                return addPlotViewCommand;
            }
        }

        private void AddPlot(object param)
        {
            PlotModel mdl = new PlotModel(RandomString(5));
            LineSeries line1 = new LineSeries();
            ChartValues<double> values = new ChartValues<double>();
            for (int i = 0; i < 50; i++)
            {
                values.Add(random.NextDouble());
            }
            line1.Values = values;
            mdl.Series.Add(line1);
            Plots.Add(mdl);
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
