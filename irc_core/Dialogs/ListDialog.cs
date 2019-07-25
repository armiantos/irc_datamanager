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
                OnSelectEvent(originalSender, new ListDialogEventArgs(ListDialogEventArgs.EventType.Select,
                    value));
                OnPropertyChanged("Selected");
                Close();
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

        public delegate void SelectEventHandler(object sender, ListDialogEventArgs e);

        public event SelectEventHandler OnSelectEvent;
    }

    public class ListDialogEventArgs
    {
        private object message;
        private EventType type;

        public enum EventType
        {
            Select,
        }

        public ListDialogEventArgs(object message)
        {
            this.message = message;
        }

        public ListDialogEventArgs(EventType type, object message)
        {
            this.type = type;
            this.message = message;
        }

        public EventType Type
        {
            get
            {
                return type;
            }
        }

        public object Message
        {
            get
            {
                return message;
            }
        }
        
    }
}
