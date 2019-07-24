using irc_core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSharedLibrary;

namespace irc_core.ViewModels
{
    public class DataSourceViewModel : ObservableObject
    {
        private string name;

        public ObservableCollection<PlotModel> Plots { get; }

        public DataSourceViewModel(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
    }
}
