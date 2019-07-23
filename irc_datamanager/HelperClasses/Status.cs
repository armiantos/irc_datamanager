using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_datamanager.HelperClasses
{
    public class Status
    {
        public delegate void StatusUpdateEventHandler(string message, int priority);

        public static event StatusUpdateEventHandler OnUpdate;

        public static void Update(string message, int priority)
        {
            OnUpdate(message, priority);
        }
    }
}
