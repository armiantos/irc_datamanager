using System;
using System.Windows.Input;

namespace WpfSharedLibrary
{
    public class CommandWrapper : ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExecute;

        public CommandWrapper(Action<object> execute) : this(execute, null) { }

        public CommandWrapper(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
