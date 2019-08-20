using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSharedLibrary;

namespace irc_core.DataSources
{
    public abstract class DataSource : ObservableObject
    {
        private string label;

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
                OnPropertyChanged("Label");
            }
        }

    }
}
