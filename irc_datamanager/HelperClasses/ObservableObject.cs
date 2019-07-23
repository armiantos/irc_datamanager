using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_datamanager.HelperClasses
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
