using ControlsCommon.Models;
using ControlsCommon.ViewModels.Pickers;

namespace ControlsCommon.ControlsViews
{
    public interface IPickerView
    {
        void ValueChanged();
    }

    public interface IPickerView<T> : IPickerView
    {
        IVMPicker<T>? ViewModel { get; }
        T Value { get; set; }
        #region [Events]
        event Action<IMPicker<T>>? AskUserForInputEvent;
        #endregion
    }

    public interface IPickerSettingsView<T> : IPickerView<T>, ISettings
    { }
}