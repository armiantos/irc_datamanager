using WpfSharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace irc_connector.ViewModels
{
    public class OperationsViewModel : ViewModel
    {
        private ICommand uploadCommand;

        public ICommand UploadCommand
        {
            get
            {
                if (uploadCommand == null)
                    uploadCommand = new CommandWrapper(param =>
                    StartUpload());
                return uploadCommand;
            }
        }

        private void StartUpload()
        {
            throw new NotImplementedException();
        }
    }
}
