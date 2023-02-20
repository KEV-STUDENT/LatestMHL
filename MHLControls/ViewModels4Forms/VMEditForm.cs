using MHLCommands;
using MHLCommon.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MHLControls.ViewModels4Forms
{
    public class VMEditForm :ViewModel
    {

        private bool notBusy = true;
        #region [Constructors]
        public VMEditForm()
        {
            CloseCommand = new RelayCommand(ExecuteCloseCommand, CanExecuteCloseCommand);
            RunCommand = new RelayCommand(ExecuteRunCommand, CanExecuteRunCommand);

            RunCommandAsync = new AsyncCommand(ExecuteRunCommandAsync, CanExecuteRunCommandAsync);
        }
        #endregion

        #region [Delegates]
        private Action? CloseAction;
        private Action? RunAction;
        private Func<object?,Task>? RunAsyncFunction;
        #endregion

        #region [Events]
        public event Action? Run
        {
            add => RunAction += value;
            remove => RunAction -= value;
        }

        public event Action? Close
        {
            add => CloseAction += value;
            remove => CloseAction -= value;
        }

        public event Func<object?,Task>? RunAsync
        {
            add => RunAsyncFunction += value;
            remove => RunAsyncFunction -= value; 
        }
        #endregion

        #region [Properties]
        public ICommand CloseCommand { get; set; }
        public ICommand RunCommand { get; set; }
        public IAsyncCommand RunCommandAsync { get; set; }       
        public bool NotBusy{ 
            get => notBusy; 
            set{
                notBusy = value;
                OnPropertyChanged("NotBusy");
            }
        }
        #endregion

        #region [Private Methods]
        private void ExecuteCloseCommand(object? obj)
        { CloseAction?.Invoke(); }
        private bool CanExecuteCloseCommand(object? obj)
        { return CloseAction != null; }

        private void ExecuteRunCommand(object? obj)
        { RunAction?.Invoke(); }
        private bool CanExecuteRunCommand(object? obj)
        { return RunAction != null; }

        private bool CanExecuteRunCommandAsync(object? obj)
        {
            return RunAsyncFunction != null;
        }

        private async Task ExecuteRunCommandAsync(object? obj)
        {
            await RunAsyncFunction?.Invoke(obj);
        }
        #endregion

    }
}
