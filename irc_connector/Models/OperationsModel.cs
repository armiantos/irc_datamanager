using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfSharedLibrary;

namespace irc_connector.Models
{
    public class OperationsModel : ObservableObject
    {
        private Visibility samplingRateVisibility;

        public Visibility SamplingRateVisibility
        {
            get
            {
                return samplingRateVisibility;
            }
            set
            {
                samplingRateVisibility = value;
                OnPropertyChanged("SamplingRateVisibility");
            }
        }
    }
}
