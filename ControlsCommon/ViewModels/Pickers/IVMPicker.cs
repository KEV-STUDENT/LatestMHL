using ControlsCommon.Models;
using MHLCommon.MHLScanner;
using System.Windows.Input;

namespace ControlsCommon.ViewModels.Pickers
{
    public interface IVMPicker
    {
        event Action ValueChanged;       
        protected void ExecuteAskUserEntryCommand(object? obj);
        protected bool CanExecuteAskUserEntryCommand(object? obj);
        void ValueChangedInform();
    }

    public interface IVMPicker<T> : IVMPicker
    {
        T Value { get; set; }
        event Action<IMPicker<T>> AskUserEntry;
        ICommand AskUserEntryCommand { get; set; }
    }
}