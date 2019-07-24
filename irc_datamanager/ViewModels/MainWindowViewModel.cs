using irc_connector.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_connector.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private ViewModel currentSourceViewModel;
        private ViewModel currentSinkViewModel;
        private ViewModel operationsViewModel;

        private List<ViewModel> sourceViewModels;
        private List<ViewModel> sinkViewModels;

        private List<string> sourceTypes;

        private string currentSourceName;

        public MainWindowViewModel()
        {
            sourceViewModels = new List<ViewModel>();
            sinkViewModels = new List<ViewModel>();

            // initialize source viewmodels
            OpcDaViewModel opcDaViewModel = new OpcDaViewModel();
            WinAppViewModel winAppViewModel = new WinAppViewModel();
            sourceViewModels.Add(opcDaViewModel);
            sourceViewModels.Add(winAppViewModel);

            // register source types to combobox via binding
            sourceTypes = new List<string>(sourceViewModels.Count);
            foreach (ViewModel sourceViewModel in sourceViewModels)
            {
                sourceTypes.Add(sourceViewModel.ViewModelName);
            }

            // initialize sink viewmodels
            DbViewModel dbViewModel = new DbViewModel();
            CurrentSinkViewModel = dbViewModel;

            // initialize OperationsViewModel
            OperationsViewModel operationsViewModel = new OperationsViewModel();
            OperationsViewModel = operationsViewModel;
        }

        private void ChangeSourceViewModel(string viewModelName)
        {
            CurrentSourceViewModel = sourceViewModels.FirstOrDefault(viewModel =>
                                        viewModel.ViewModelName == viewModelName);
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
                OnPropertyChanged("CurrentSourceViewModel");
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
                OnPropertyChanged("CurrentSinkViewModel");
            }
        }

        public ViewModel OperationsViewModel
        {
            get
            {
                return operationsViewModel;
            }
            set
            {
                operationsViewModel = value;
                OnPropertyChanged("OperationViewModel");
            }
        }

        public List<string> SourceTypes
        {
            get
            {
                return sourceTypes;
            }
        }

        public string CurrentSourceName
        {
            get
            {
                return currentSourceName;
            }
            set
            {
                currentSourceName = value;
                ChangeSourceViewModel(currentSourceName);
            }
        }
    }
}
