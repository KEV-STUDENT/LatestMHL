using MHLCommon;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MHLControls.MHLPickers
{
    public class MHLUIPickerViewModel : ViewModel
    {

        #region [Constructors]
        public MHLUIPickerViewModel()
        {
            AskUserEntryCommand = new RelayCommand(ExecuteAskUserEntryCommand, CanExecuteAskUserEntryCommand);
        }
        #endregion

        #region [Events]
        public event Action ValueChanged
        {
            add { ValueChangedAction += value; }
            remove { ValueChangedAction -= value; }
        }
        #endregion

        #region [Delegates]
        private Action? ValueChangedAction;
        public Action? AskUserEntryAction;
        #endregion

        #region [Properties]
        public ICommand AskUserEntryCommand { get; set; }
        #endregion

        #region [Private Methods]
        private void ExecuteAskUserEntryCommand(object? obj)
        { AskUserEntryAction?.Invoke(); }
        private bool CanExecuteAskUserEntryCommand(object? obj)
        { return AskUserEntryAction != null; }
        #endregion


        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
                ValueChangedAction?.Invoke();
            }
        }
    }
}
