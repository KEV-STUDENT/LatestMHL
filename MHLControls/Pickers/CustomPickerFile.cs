namespace MHLControls.Pickers
{
    public class CustomPickerFile :CustomPickerString
    {
        public CustomPickerFile():base() {
            AskUserForInputEvent += MHLAsk4Picker.AskFile;
        }
    }
}
