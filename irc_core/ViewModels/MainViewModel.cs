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
    
        public static SnackbarMessageQueue MessageQueue { get; private set; }

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
            CustomDialog.Show(dialog, DialogClosingEventHandler);   
        }


        private void DialogClosingEventHandler(object sender, ClosingEventArgs args)
        {
            if (args.Parameter is object[])
            {
                object[] parameters = (object[])args.Parameter;
                if(parameters[0] is AddDbSourceDialog)
                {
                    AddDbSourceDialog o = (AddDbSourceDialog)parameters[0];
                    PasswordBox p = (PasswordBox)parameters[1];
                    DataSources.Add(new DbConnection(o.SelectedDb, o.Host, o.Username, p.Password)
                    {
                        Label = $"{o.SelectedDb} @ {o.Host}"
                    });
                }
            }
        }
        #endregion
    }
}
