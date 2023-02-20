using MHLControls.Pickers;

namespace MHLUIElements
{
    public class PickerMSSqlServerDB : CustomPickerSettingsString
    {
        public PickerMSSqlServerDB()
        {
            _vm = new ViewModel4PickerMSSqlServerDB(this);         
        }
    }
}
