using System;
using MHLCommon.ViewModels;
using System.Windows.Input;
using MHLCommands;

namespace MHLControls.ViewModels4Forms
{
    public class VMEditForm :ViewModel
    {
        #region [Constructors]
        public VMEditForm()
        {
            CloseCommand = new RelayCommand(ExecuteCloseCommand, CanExecuteCloseCommand);
            RunCommand = new RelayCommand(ExecuteRunCommand, CanExecuteRunCommand);
        }
        #endregion

        #region [Delegates]
        private Action? CloseAction;
        private Action? RunAction;
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
        #endregion

        #region [Properties]
        public ICommand CloseCommand { get; set; }
        public ICommand RunCommand { get; set; }
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
        #endregion

    }
}
