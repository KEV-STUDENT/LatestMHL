using ControlsCommon;
using ControlsCommon.ControlsViews;
using System;

namespace MHLControls.Pickers
{
    public abstract class CustomPickerSettings<T> : CustomPicker<T>, IPickerSettingsView<T>
    { 
        #region [Events]
        public event Action? AskUserForSettings
        {
            add
            {
                if (ViewModel is ISettings vm)
                    vm.AskUserForSettings += value;
            }
            remove
            {
                if (ViewModel is ISettings vm)
                    vm.AskUserForSettings -= value;
            }
        }
        #endregion

        #region [ISettings]
        event Action? ISettings.AskUserForSettings
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
