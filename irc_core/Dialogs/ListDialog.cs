using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace irc_core.Dialogs
{
    public class ListDialog : Dialog
    {
        public ObservableCollection<string> ItemList { get; set; }

        private string selected;

        private object originalSender;

        public string Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
                Close(selected);
            }
        }

        public ListDialog(object originalSender)
        {
            this.originalSender = originalSender;
            ItemList = new ObservableCollection<string>();
        }

        public ListDialog(object originalSender, ICollection<string> itemList)
        {
            this.originalSender = originalSender;
            ItemList = new ObservableCollection<string>(itemList);
        }
    }
}
