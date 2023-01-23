using System.Windows.Input;

namespace ControlsCommon.ViewModels
{
    public interface IVMPicker
    {
        ICommand AskUserEntryCommand { get; set; }
        protected void ExecuteAskUserEntryCommand(object? obj);        
        protected bool CanExecuteAskUserEntryCommand(object? obj);
    }

    public interface IVMPicker<T> : IVMPicker
    {
    }
}