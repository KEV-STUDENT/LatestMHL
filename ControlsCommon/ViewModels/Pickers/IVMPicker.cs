using ControlsCommon.Models;
using System.Windows.Input;

namespace ControlsCommon.ViewModels.Pickers
{
    public interface IVMPicker
    {
        event Action ValueChanged;       
        protected void ExecuteAskUserEntryCommand(object? obj);
        protected bool CanExecuteAskUserEntryCommand(object? obj);
        void ValueChangedInform();
        void IsReadOnlyTextInputChangedInform();
    }

    public interface IVMPicker<T> : IVMPicker
    {
        T Value { get; set; }
        event Action<IMPicker<T>> AskUserEntry;
        ICommand AskUserEntryCommand { get; set; }
    }
    public interface IVMPickerSettings<T> : IVMPicker<T>, ISettings
    {
        ICommand AskUserSettingsCommand { get; set; }
        protected void ExecuteAskUserSettingsCommand(object? obj);
        protected bool CanExecuteAskUserSettingsCommand(object? obj);
    }
}