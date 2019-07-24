using irc_core.Models;
using LiveCharts;
using LiveCharts.Defaults;
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

        private ICommand closePlotViewCommand;

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

        public ICommand ClosePlotViewCommand
        {
            get
            {
                if (closePlotViewCommand == null)
                    closePlotViewCommand = new CommandWrapper(param =>
                    RemovePlot((PlotModel)param));
                return closePlotViewCommand;
            }
        }

        private void AddPlot(object param)
        {
            PlotModel mdl = new PlotModel(RandomString(5));
            for (int i = 0; i < 2; i++)
            {
                LineSeries line = new LineSeries();
                line.Title = $"line#{i}";
                ChartValues<double> values = new ChartValues<double>();
                for (int j = 0; j < 30; j++)
                {
                    values.Add(random.NextDouble());
                }
                line.Values = values;
                mdl.Series.Add(line);
            }
            Plots.Add(mdl);
        }

        private void RemovePlot(PlotModel toBeRemoved)
        {
            Plots.Remove(toBeRemoved);
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
