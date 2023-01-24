using ControlsCommon.ControlsViews;
using MHLCommands;
using System.Windows.Input;

namespace ControlsCommon.ViewModels.Pickers
{
    public class VMPickerSettings<T> : VMPicker<T>, IVMPickerSettings<T>
    {
        #region [Delegates]
        private Action? askUserSettings;
        #endregion

        #region [Constructors]
        public VMPickerSettings(IPickerSettingsView<T> viewUI) : base(viewUI) 
        {
            AskUserSettingsCommand = new RelayCommand(ExecuteAskUserSettingsCommand, CanExecuteAskUserSettingsCommand);
        }
        #endregion

        #region [Properties]
        public ICommand AskUserSettingsCommand { get; set; }
        #endregion

        #region [Events]
        public event Action? AskUserSettings
        {
            add
            {
                askUserSettings += value;
            }

            remove
            {
                askUserSettings -= value;
            }
        }
        #endregion

        #region [Methods]
        private bool CanExecuteAskUserSettingsCommand(object? obj)
        { return askUserSettings != null; }

        private void ExecuteAskUserSettingsCommand(object? obj)
        { askUserSettings?.Invoke(); }
        #endregion

        #region [IVMPickerSettings]
        ICommand IVMPickerSettings<T>.AskUserSettingsCommand { get => AskUserSettingsCommand; set => AskUserSettingsCommand = value; }
        #endregion

        #region [ISettings]
        event Action? ISettings.AskUserSettings
        {
            add
            {
                AskUserSettings += value;
            }

            remove
            {
                AskUserSettings -= value;
            }
        }

        bool IVMPickerSettings<T>.CanExecuteAskUserSettingsCommand(object? obj)
        {
            return CanExecuteAskUserSettingsCommand(obj);
        }

        void IVMPickerSettings<T>.ExecuteAskUserSettingsCommand(object? obj)
        {
            ExecuteAskUserSettingsCommand(obj);
        }
        #endregion
    }
}
