using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSharedLibrary;

namespace irc_core.Models
{
    public abstract class DataModel : ObservableObject
    {
        public List<string> Tags { get; set; }

        public bool IsLive { get; set; }

        public DataModel()
        {
            IsLive = true;
        }
    }
}
