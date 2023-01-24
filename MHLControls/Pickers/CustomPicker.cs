using ControlsCommon.ControlsViews;
using ControlsCommon.Models;
using ControlsCommon.ViewModels.Pickers;
using System;
using System.Windows;
using System.Windows.Input;

namespace MHLControls.Pickers
{
    public class CustomPicker : UICommandControl, IPickerView<string>
    {
        private IVMPicker<string>? _vm;
        public static readonly DependencyProperty ValueProperty;

        #region [Constructors]
        static CustomPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomPicker), new FrameworkPropertyMetadata(typeof(CustomPicker)));
            ValueProperty = DependencyProperty.Register(
                "Value",
                typeof(String),
                typeof(CustomPicker),
                new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(CurrentValueChanged)));
        }

        public CustomPicker()
        {
            _vm = new VMPicker<string>(this);            
        }
        #endregion

        #region [Properties]
        public IVMPicker<string>? ViewModel => _vm;

        public string Value
        {
            get {
                return (String)GetValue(ValueProperty); 
            }
            set { 
                SetValue(ValueProperty, value); 
            }
        }
        #endregion

        #region [Events]
        public event Action<IMPicker<string>>? AskUserForInputEvent
        {
            add {
                if(ViewModel != null)
                    ViewModel.AskUserEntry += value; 
            }
            remove {
                if (ViewModel != null)
                    ViewModel.AskUserEntry -= value; 
            }
        }
        #endregion

        #region [Methods]
        private static void CurrentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if ((d is CustomPicker picker) && picker != null)
            {
                picker?.ViewModel?.ValueChangedInform();
            }
        }
        #endregion


        #region [IVMPicker]
        void IPickerView.ValueChanged()
        {
            GenerateCommand();
        }

        IVMPicker<string>? IPickerView<string>.ViewModel => ViewModel;
        string IPickerView<string>.Value {
            get
            {
                return Value;
            }
            set { 
                Value = value; 
            } 
        }
        event Action<IMPicker<string>>? IPickerView<string>.AskUserForInputEvent
        {
            add
            {
                AskUserForInputEvent += value;
            }

            remove
            {
                AskUserForInputEvent -= value;
            }
        }

        #endregion
    }
}
