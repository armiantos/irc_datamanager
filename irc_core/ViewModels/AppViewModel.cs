using irc_core.DataSources;
using irc_core.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.ViewModels
{
    public class AppViewModel : ObservableObject
    {
        #region privates

        private ICommand addDataSourceCommand;

        public ObservableCollection<DataSource> DataSources { get; set; }

        #endregion
               
        #region public getters, setters


        public ICommand AddDataSourceCommand
        {
            get
            {
                if (addDataSourceCommand == null)
                    addDataSourceCommand = new CommandWrapper(param =>
                    AddDataSource());
                return addDataSourceCommand;
            }
        }

        #endregion

        #region methods

        public AppViewModel()
        {

            DataSources = new ObservableCollection<DataSource>();
        }

        private void AddDataSource()
        {
            AddDataSourceDialog dialog = new AddDataSourceDialog();
            Dialog.Show(dialog, DialogClosingEventHandler);
        }

        private void DialogClosingEventHandler(object sender, ClosingEventArgs args)
        {
            if (sender is AddDataSourceDialog)
            {
                AddDataSourceDialog originalSender = (AddDataSourceDialog)sender;

            }
        }

        //private void ListDialogClosingHandler(object sender, DialogClosingEventArgs eventArgs)
        //{
        //    if (eventArgs.Session.Content is AddDataSourceDialog)
        //    {
        //        if (eventArgs.Parameter != null)
        //        {
        //            if (eventArgs.Parameter is object[])
        //            {
        //                object[] param = (object[])eventArgs.Parameter;
        //                if (param[0] is AddDatabaseSource)
        //                {
        //                    AddDatabaseSource dbSource = (AddDatabaseSource)param[0];
        //                    PasswordBox pwdBox = (PasswordBox)param[1];
        //                    DataSources.Add(new DatabaseSource(dbSource.SelectedDb,
        //                        dbSource.Host,
        //                        dbSource.Username,
        //                        pwdBox.Password)
        //                        { Label = $"{dbSource.SelectedDb} @ {dbSource.Host}" });
        //                }
        //            }
        //        }
        //    }           
        //}
        #endregion
    }
}
