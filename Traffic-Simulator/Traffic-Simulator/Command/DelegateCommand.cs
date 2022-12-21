using System;
using System.Windows.Input;

namespace Traffic_Simulator.Command
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action<object> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object? parameter) => _canExecute is null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);
    }
}
