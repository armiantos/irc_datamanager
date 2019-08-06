using irc_core.DataSources;
using irc_core.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.ViewModels
{
    public class MainViewModel : ObservableObject
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
                    addDataSourceCommand = new RelayCommand(param =>
                    AddDataSource());
                return addDataSourceCommand;
            }
        }
        public SnackbarMessageQueue MessageQueue { get; set; }

        #endregion

        #region methods

        public MainViewModel()
        {

            DataSources = new ObservableCollection<DataSource>();
            MessageQueue = new SnackbarMessageQueue();
        }

        private void AddDataSource()
        {
            AddDataSourceDialog dialog = new AddDataSourceDialog();
            Dialog.Show(dialog, DialogClosingEventHandler);
        }


        private void DialogClosingEventHandler(object sender, ClosingEventArgs args)
        {
            if (args.Parameter is object[])
            {
                object[] parameters = (object[])args.Parameter;
                if(parameters[0] is AddDatabaseSource)
                {
                    AddDatabaseSource o = (AddDatabaseSource)parameters[0];
                    PasswordBox p = (PasswordBox)parameters[1];
                    DataSources.Add(new DatabaseSource(o.SelectedDb, o.Host, o.Username, p.Password)
                    {
                        Label = $"{o.SelectedDb} @ {o.Host}"
                    });
                }
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
