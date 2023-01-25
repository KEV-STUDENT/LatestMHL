using ControlsCommon.ControlsViews;
using ControlsCommon.Models;
using ControlsCommon.ViewModels.Pickers;
using System;
using System.Windows;

namespace MHLControls.Pickers
{
    public abstract class CustomPicker<T> : UICommandControl, IPickerView<T>
    {
        #region [Properties]
        abstract public IVMPicker<T>? ViewModel { get; }
        abstract public T Value { get; set; }
        abstract public bool IsReadOnlyTextInput { get; set; }
        #endregion

        #region [Events]
        public event Action<IMPicker<T>>? AskUserForInputEvent
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
        protected static void CurrentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if ((d is CustomPicker<T> picker) && picker != null)
            {
                picker?.ViewModel?.ValueChangedInform();
            }
        }

        protected static void IsReadOnlyTextInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if ((d is CustomPicker<T> picker) && picker != null)
            {
                picker?.ViewModel?.IsReadOnlyTextInputChangedInform();
            }
        }
        #endregion

        #region [IVMPicker]
        void IPickerView.ValueChanged()
        {
            GenerateCommand();
        }
        IVMPicker<T>? IPickerView<T>.ViewModel => ViewModel;
        bool IPickerView.IsReadOnlyTextInput { get => IsReadOnlyTextInput; set => IsReadOnlyTextInput = value; }
        T IPickerView<T>.Value {
            get
            {
                return Value;
            }
            set { 
                Value = value; 
            } 
        }
        event Action<IMPicker<T>>? IPickerView<T>.AskUserForInputEvent
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
