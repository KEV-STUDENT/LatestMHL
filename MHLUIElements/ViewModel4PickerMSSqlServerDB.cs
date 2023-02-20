using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels.Pickers;

namespace MHLUIElements
{
    internal class ViewModel4PickerMSSqlServerDB : VMPickerSettings<string>
    {
        
        public ViewModel4PickerMSSqlServerDB(IPickerSettingsView<string> viewUI) : base(viewUI) {
            AskUserSettings += () => {
                MSSQLSettings.MSSQLServerSettings settings = new MSSQLSettings.MSSQLServerSettings();
                settings.ShowDialog();
            };
        }
    }
}