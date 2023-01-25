namespace MHLControls.Pickers
{
    public class CustomPickerSettingsFile :CustomPickerSettingsString
    {
        public CustomPickerSettingsFile():base() {
            AskUserForInputEvent += MHLAsk4Picker.AskFile;
        }
    }
}
