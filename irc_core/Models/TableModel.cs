using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.Models
{
    public class TableModel : DataModel
    {
        private DataView dataView;

        public DataView DataView
        {
            get
            {
                return dataView;
            }
            set
            {
                dataView = value;
                OnPropertyChanged("Data");
            }
        }
    }
}
