using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.Dialogs
{
    public class ListDialog : Dialog
    {
        public ObservableCollection<string> ItemList { get; set; }
        public ListDialog()
        {
            ItemList = new ObservableCollection<string>();
        }

        public ListDialog(ICollection<string> itemList)
        {
            ItemList = new ObservableCollection<string>(itemList);
        }
    }
}
