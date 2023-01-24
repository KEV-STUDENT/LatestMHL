using ControlsCommon.ViewModels.Pickers;
using MHLControls.MHLPickers;
using System;
using System.Windows;
namespace MHLControls.Pickers
{
    public class CustomPickerDirectory : CustomPicker<string>
    {
        public static readonly DependencyProperty ValueProperty;

        #region [Constructors]
        static CustomPickerDirectory()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomPickerDirectory), new FrameworkPropertyMetadata(typeof(CustomPickerDirectory)));

            ValueProperty = DependencyProperty.Register(
                "Value",
                typeof(String),
                typeof(CustomPickerDirectory),
                new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(CurrentValueChanged)));
        }

        public CustomPickerDirectory()
        {
            _vm = new VMPicker<string>(this);
            AskUserForInputEvent += MHLAsk4Picker.AskDirectory;
        }
        #endregion

        #region [Properties]
        public override string Value { 
            get {
                return (string) GetValue(ValueProperty);
             }
            set { 
                SetValue(ValueProperty, value);
            }
        }
        #endregion
    }
}
