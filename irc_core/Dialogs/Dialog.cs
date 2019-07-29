using System;
using WpfSharedLibrary;

namespace irc_core.Dialogs
{
    public class Dialog : ObservableObject
    {
        public async void Show()
        {
            throw new NotImplementedException();
            //await DialogHost.Show(this);
        }

        //public async void Show(DialogClosingEventHandler dialogClosingEventHandler)
        //{
        //    await DialogHost.Show(this, dialogClosingEventHandler);
        //}

        public void Close()
        {
            throw new NotImplementedException();
            //DialogHost.CloseDialogCommand.Execute(null, null);
        }

        public void Close(bool success)
        {
            throw new NotImplementedException();
            //DialogHost.CloseDialogCommand.Execute(success, null);
        }

        public void Close(object param)
        {
            throw new NotImplementedException();
            //DialogHost.CloseDialogCommand.Execute(param, null);
        }
    }
}
