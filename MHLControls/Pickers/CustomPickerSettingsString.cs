using ControlsCommon.ViewModels.Pickers;
using System.Windows;
using System;

namespace MHLControls.Pickers
{
    public class CustomPickerSettingsString : CustomPickerSettings<string>
    {

        protected IVMPickerSettings<string> _vm;
        public static readonly DependencyProperty ValueProperty;
        public static readonly DependencyProperty IsReadOnlyTextInputProperty;

        #region [Constructors]
        static CustomPickerSettingsString()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomPickerSettingsString), new FrameworkPropertyMetadata(typeof(CustomPickerSettingsString)));

            ValueProperty = DependencyProperty.Register(
                "Value",
                typeof(String),
                typeof(CustomPickerSettingsString),
                new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(CurrentValueChanged)));

            IsReadOnlyTextInputProperty = DependencyProperty.Register(
               "IsReadOnlyTextInput",
               typeof(bool),
               typeof(CustomPickerSettingsString),
               new UIPropertyMetadata(false, new PropertyChangedCallback(IsReadOnlyTextInputChanged)));
        }

        public CustomPickerSettingsString()
        {
            _vm = new VMPickerSettings<string>(this);           
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
        public override IVMPickerSettings<string>? ViewModel => _vm;
        #endregion
    }
}
