using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MHLCommands
{
    public abstract class AsyncCommandBase : IAsyncCommand
    {
        #region [ICommand implementation]
        event EventHandler? ICommand.CanExecuteChanged
        {
            add
            {
                CanExecuteChanged += value;
            }

            remove
            {
                CanExecuteChanged -= value;
            }
        }

        bool ICommand.CanExecute(object? parameter)
        {
            return CanExecute(parameter);
        }

        async void ICommand.Execute(object? parameter)
        {           
            await ExecuteAsync(parameter);
        }
        #endregion

        #region [IAsyncCommand implementation]
        Task IAsyncCommand.ExecuteAsync(object? parameter)
        {
            return ExecuteAsync(parameter);
        }
        #endregion

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract bool CanExecute(object? parameter);
        public abstract Task ExecuteAsync(object? parameter);

        public async void Execute(object? parameter)
        {
            await ExecuteAsync(parameter);
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
