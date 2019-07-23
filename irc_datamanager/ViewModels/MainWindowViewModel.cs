using irc_datamanager.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_datamanager.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private ViewModel currentSourceViewModel;
        private ViewModel currentSinkViewModel;
        private ViewModel operationsViewModel;

        public MainWindowViewModel()
        {
            
        }

        public ViewModel CurrentSourceViewModel
        {
            get
            {
                return currentSourceViewModel;
            }
            set
            {
                currentSourceViewModel = value;
                OnPropertyChanged(this, "CurrentSourceViewModel");
            }
        }

        public ViewModel CurrentSinkViewModel
        {
            get
            {
                return currentSinkViewModel;
            }
            set
            {
                currentSinkViewModel = value;
                OnPropertyChanged(this, "CurrentSinkViewModel");
            }
        }

        public ViewModel OperationViewModel
        {
            get
            {
                return operationsViewModel;
            }
            set
            {
                operationsViewModel = value;
                OnPropertyChanged(this, "OperationViewModel");
            }
        }
    }
}
