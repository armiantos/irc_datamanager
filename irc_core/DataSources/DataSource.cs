using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSharedLibrary;

namespace irc_core.DataSources
{
    public abstract class DataSource : ObservableObject
    {
        public delegate void DataSourceEventHandler(object sender, DataSourceEventArgs args);

        public event DataSourceEventHandler OnDataSourceEvent;

        public void NotifyDataSourceEvent(object sender, DataSourceEventArgs message)
        {
            OnDataSourceEvent(sender, message);
        }
    }

    public class DataSourceEventArgs
    {
        private object message;
        private MessageType msgType;
        private EventType type;

        public enum EventType
        {
            Database
        }

        public enum MessageType
        {
            SpaceList,
            CollectionList
        }

        public DataSourceEventArgs(object message)
        {
            this.message = message;
        }

        public DataSourceEventArgs(EventType type, object message)
        {
            this.type = type;
            this.message = message;
        }

        public DataSourceEventArgs(EventType type, MessageType msgType, object message)
        {
            this.type = type;
            this.message = message;
            this.msgType = msgType;
        }


        public EventType Type
        {
            get
            {
                return type;
            }
        }

        public MessageType MsgType
        {
            get
            {
                return msgType;
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
