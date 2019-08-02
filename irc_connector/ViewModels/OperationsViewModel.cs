using WpfSharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using irc_connector.Models;
using System.Windows;

namespace irc_connector.ViewModels
{
    public class OperationsViewModel : ViewModel
    {
        OperationsModel model;

        private ICommand uploadCommand;

        public ICommand UploadCommand
        {
            get
            {
                if (uploadCommand == null)
                    uploadCommand = new RelayCommand(param =>
                    StartUpload());
                return uploadCommand;
            }
        }

        private void StartUpload()
        {
            throw new NotImplementedException();
        }

        public OperationsViewModel()
        {
            model = new OperationsModel();
            model.SamplingRateVisibility = Visibility.Collapsed;
        }

        public OperationsModel Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                OnPropertyChanged("Model");
            }
        }
    }
}
