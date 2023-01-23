using ControlsCommon.ControlsViews;

namespace ControlsCommon.ViewModels
{
    public class VMPicker :IVMPicker
    {
        private IPickerView view;

        public VMPicker(IPickerView customPicker)
        {
            this.view = customPicker;
        }
    }
}