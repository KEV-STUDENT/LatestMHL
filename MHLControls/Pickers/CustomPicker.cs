using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels;
using System.Windows;

namespace MHLControls.Pickers
{
    public class CustomPicker : UICommandControl, IPickerView
    {
        private IVMPicker _vm;

        #region [Constructors]
        static CustomPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomPicker), new FrameworkPropertyMetadata(typeof(CustomPicker)));
        }

        public CustomPicker()
        {
            _vm = new VMPicker(this);
        }
        #endregion
    }
}
