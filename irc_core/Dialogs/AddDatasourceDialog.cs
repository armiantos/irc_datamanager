using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.Dialogs
{
    public class AddDataSourceDialog : Dialog
    {
        public List<string> supportedTypes;

        private string selectedType;

        public List<string> SupportedTypes
        {
            get
            {
                return supportedTypes;
            }
        }

        public string SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;
                    OnPropertyChanged("SelectedType");
                }
            }
        }
    }
}
