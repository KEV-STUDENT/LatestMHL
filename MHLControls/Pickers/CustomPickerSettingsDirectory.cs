namespace MHLControls.Pickers
{
    public class CustomPickerSettingsDirectory : CustomPickerSettingsString
    {
        public CustomPickerSettingsDirectory() : base()
        {
            AskUserForInputEvent += MHLAsk4Picker.AskDirectory;
        }
    }
}
