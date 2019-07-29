using irc_core.DataSources;
using irc_core.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using WpfSharedLibrary;

namespace irc_core.ViewModels
{
    public class AppViewModel : ObservableObject
    {
        #region privates

        private ICommand addDataSourceCommand;

        private Dialog currentDialogHost;

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


        public Dialog CurrentDialogHost
        {
            get
            {
                return currentDialogHost;
            }
            set
            {
                currentDialogHost = value;
                OnPropertyChanged("CurrentDialogHost");
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
            CurrentDialogHost = new AddDataSourceDialog();
            CurrentDialogHost.Show(ListDialogClosingHandler);
        }

        private void ListDialogClosingHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Session.Content is AddDataSourceDialog)
            {
                if (eventArgs.Parameter != null)
                {
                    if (eventArgs.Parameter is object[])
                    {
                        object[] param = (object[])eventArgs.Parameter;
                        if (param[0] is AddDatabaseSource)
                        {
                            AddDatabaseSource dbSource = (AddDatabaseSource)param[0];
                            PasswordBox pwdBox = (PasswordBox)param[1];
                            DataSources.Add(new DatabaseSource(dbSource.SelectedDb,
                                dbSource.Host,
                                dbSource.Username,
                                pwdBox.Password)
                                { Label = $"{dbSource.SelectedDb} @ {dbSource.Host}" });
                        }
                    }
                }
            }           
        }

        public async void TableDialogClosingHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Session.Content is AddDataViewDialog)
            {
                if(eventArgs.Parameter != null && (bool)eventArgs.Parameter == true)
                {
                    DatabaseCollection originalSender = (DatabaseCollection)((AddDataViewDialog)eventArgs.Session.Content).OriginalSender;
                    AddDataViewDialog dialog = (AddDataViewDialog)eventArgs.Session.Content;
                    string type = dialog.SupportedViews.FirstOrDefault(obj => obj.Boolean == true).Label;
                    if (!string.IsNullOrEmpty(type))
                        await originalSender.AddDataView(type, dialog.GetIncluded()); 
                }
            }
        }

        #endregion
    }
}
