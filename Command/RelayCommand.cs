using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskList.Command
{
#pragma warning disable CS0067, CS8604
    public class RelayCommand(Action<object> execute, Predicate<object>? canExecute = null) : ICommand
    {
        public Action<object> execute = execute ?? throw new ArgumentNullException(nameof(execute));
        public Predicate<object> canExecute = canExecute ?? (_ => true);

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter) => execute(parameter);
        public bool CanExecute(object? parameter) => canExecute(parameter);   
    }
}
#pragma warning restore CS0067, CS8904