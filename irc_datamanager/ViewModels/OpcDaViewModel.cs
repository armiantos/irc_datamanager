using irc_datamanager.HelperClasses;
using irc_datamanager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace irc_datamanager.ViewModels
{
    public class OpcDaViewModel : ViewModel
    {
        private ICommand connectToHostCommand;

        public override string ViewModelName
        {
            get
            {
                return "OPC DA";
            }
        }

        public OpcDaModel OpcDaModel { get; }

        public ICommand ConnectToHostCommand
        {
            get
            {
                if (connectToHostCommand == null)
                {
                    connectToHostCommand = new CommandWrapper(param =>
                    {
                        ConnectToOpcServer();
                    });
                }
                return connectToHostCommand;
            }
        }
        
        private void ConnectToOpcServer()
        {
            throw new NotImplementedException();
        }

        public OpcDaViewModel()
        {
            OpcDaModel = new OpcDaModel();
        }
    }
}
