using ControlsCommon.ViewModels.Pickers;
using System;
using System.Windows;
namespace MHLControls.Pickers
{
    public class CustomPickerString : CustomPicker<string>
    {
        protected IVMPicker<string>? _vm;
        public static readonly DependencyProperty ValueProperty;
        public static readonly DependencyProperty IsReadOnlyTextInputProperty;

        #region [Constructors]
        static CustomPickerString()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomPickerString), new FrameworkPropertyMetadata(typeof(CustomPickerString)));

            ValueProperty = DependencyProperty.Register(
                "Value",
                typeof(String),
                typeof(CustomPickerString),
                new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(CurrentValueChanged)));

            IsReadOnlyTextInputProperty = DependencyProperty.Register(
                "IsReadOnlyTextInput",
                typeof(bool),
                typeof(CustomPickerString),
                new UIPropertyMetadata(false, new PropertyChangedCallback(IsReadOnlyTextInputChanged)));
        }

        public CustomPickerString()
        {
            _vm = new VMPicker<string>(this);
        }
        #endregion

        #region [Properties]
        public override string Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        public override bool IsReadOnlyTextInput
        {
            get
            {
                return (bool)GetValue(IsReadOnlyTextInputProperty);
            }
            set
            {
                SetValue(IsReadOnlyTextInputProperty, value);
            }
        }

        public override IVMPicker<string>? ViewModel => _vm;
        #endregion
    }
}
