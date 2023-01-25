namespace MHLControls.Pickers
{
    public class CustomPickerDirectory : CustomPickerString
    {
        public CustomPickerDirectory():base() {
            AskUserForInputEvent += MHLAsk4Picker.AskDirectory;
        }
    }
}
