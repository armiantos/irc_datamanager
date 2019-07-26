using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSharedLibrary;

namespace irc_core.Models
{
    public class PlotModel : DataModel
    {
        private SeriesCollection series;

        public SeriesCollection Series
        {
            get
            {
                if (series == null)
                    series = new SeriesCollection();
                return series;
            }
            set
            {
                series = value;
                OnPropertyChanged("Series");
            }
        }
    }
}
