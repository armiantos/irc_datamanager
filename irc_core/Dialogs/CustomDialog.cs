using irc_core.Views;
using System;
using System.Windows.Controls;
using WpfSharedLibrary;

namespace irc_core.Dialogs
{   
    public abstract class CustomDialog : ObservableObject
    {
        private static DialogView dialogView;

        private static CustomDialog context;

        public delegate void ClosingEventHandler(object sender, ClosingEventArgs args);

        private static ClosingEventHandler handler;

        public static void Show(CustomDialog context)
        {
            dialogView = new DialogView();
            dialogView.DataContext = context;
            CustomDialog.context = context;
            dialogView.Show();
        }

        public static void Show(CustomDialog context, ClosingEventHandler handler) 
        {
            Show(context);
            CustomDialog.handler = handler;
        }

        public static void Close()
        {
            handler?.Invoke(context, new ClosingEventArgs() { Content = dialogView.ContentControl.Content });
            dialogView.Close();
        }

        public static void Close(object param)
        {
            handler?.Invoke(context, new ClosingEventArgs(param) { Content = dialogView.ContentControl.Content });
            dialogView.Close();
        }
    }

    public class ClosingEventArgs
    { 
        public object Parameter { get; set; }

        public object Content { get; set; }

        public ClosingEventArgs()
        {

        }

        public ClosingEventArgs(object param) : base()
        {
            Parameter = param;
        }
    }
}
