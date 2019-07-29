using irc_core.Views;
using System;
using WpfSharedLibrary;

namespace irc_core.Dialogs
{

   
    public abstract class Dialog : ObservableObject
    {
        private static DialogView dialogView;

        private static Dialog context;

        public delegate void ClosingEventHandler(object sender, ClosingEventArgs args);

        private static ClosingEventHandler handler;

        public Dialog()
        {
            dialogView = new DialogView();
            dialogView.Deactivated += DialogView_Deactivated;
        }

        private void DialogView_Deactivated(object sender, EventArgs e)
        {
            Close();
        }

        public static void Show(Dialog context)
        {
            dialogView.DataContext = context;
            dialogView.Show();
        }

        public static void Show(Dialog context, ClosingEventHandler handler) 
        {
            Show(context);
         
            Dialog.context = context;
            Dialog.handler = handler;
        }

        //public async void Show(DialogClosingEventHandler dialogClosingEventHandler)
        //{
        //    await DialogHost.Show(this, dialogClosingEventHandler);
        //}

        public static void Close()
        {
            handler?.Invoke(context, new ClosingEventArgs());
            dialogView.Close();
        }

        public static void Close(object param)
        {
            throw new NotImplementedException();
        }
    }

    public class ClosingEventArgs
    {
        public object Parameters { get; set; }

        public ClosingEventArgs()
        {

        }

        public ClosingEventArgs(object param) : base()
        {
            Parameters = param;
        }
    }
}
