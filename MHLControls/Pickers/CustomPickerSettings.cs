using ControlsCommon;
using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels.Pickers;
using System;

namespace MHLControls.Pickers
{
    public abstract class CustomPickerSettings<T> : CustomPicker<T>, IPickerSettingsView<T>
    {
        override public IVMPickerSettings<T>? ViewModel { get; }
        #region [Events]
        public event Action? AskUserForSettings
        {
            add
            {
                if (ViewModel is ISettings vm)
                    vm.AskUserSettings += value;
            }
            remove
            {
                if (ViewModel is ISettings vm)
                    vm.AskUserSettings -= value;
            }
        }
        #endregion

        #region [ISettings]
        event Action? ISettings.AskUserSettings
        {
            add
            {
                AskUserForSettings += value;
            }

            remove
            {
                AskUserForSettings -= value;
            }
        }       
        #endregion
    }
}
