using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_core.HelperClasses
{
    public class Snackbar
    {
        public static SnackbarMessageQueue MessageQueue { get; set; }
        public Snackbar()
        {
            MessageQueue = new SnackbarMessageQueue();
        }

        public static void Enqueue(object content)
        {
            MessageQueue.Enqueue(content);
        }
    }
}
