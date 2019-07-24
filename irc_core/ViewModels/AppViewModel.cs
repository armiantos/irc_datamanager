using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.ViewModels
{
    public class AppViewModel : ObservableObject
    {
        private PlotViewModel plotViewModel;

        private SidePanelViewModel currentSidePanel;

        private DataSourcesViewModel dataSourcesViewModel;

        private ICommand toggleSidePanelCommand;
               
        public AppViewModel()
        {
            PlotViewModel = new PlotViewModel();

            dataSourcesViewModel = new DataSourcesViewModel();
        }

        public PlotViewModel PlotViewModel
        {
            get
            {
                return plotViewModel;
            }
            set
            {
                plotViewModel = value;
                OnPropertyChanged("PlotViewModel");
            }
        }

        public DataSourcesViewModel DataSourcesViewModel
        {
            get
            {
                return dataSourcesViewModel;
            }
        }

        public ICommand ToggleSidePanelCommand
        {
            get
            {
                if (toggleSidePanelCommand == null)
                    toggleSidePanelCommand = new CommandWrapper(param =>
                    CurrentSidePanel = (SidePanelViewModel)param);
                return toggleSidePanelCommand;
            }
        }

        public SidePanelViewModel CurrentSidePanel
        {
            get
            {
                return currentSidePanel;
            }
            set
            {
                if (currentSidePanel != value)
                {
                    currentSidePanel = value;
                }
                else
                {
                    currentSidePanel = null;
                }

                OnPropertyChanged("CurrentSidePanel");
            }
        }
    }
}
