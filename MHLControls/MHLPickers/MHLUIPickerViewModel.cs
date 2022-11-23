using MHLCommands;
using MHLCommon.ViewModels;
using System;
using System.Windows.Input;

namespace MHLControls.MHLPickers
{
    public class MHLUIPickerViewModel : ViewModel
    {
        #region [Fields]
        private string _value;
        #endregion

        #region [Properties]
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

        public ICommand AskUserEntryCommand { get; set; }
        #endregion

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

        #region [Private Methods]
        private void ExecuteAskUserEntryCommand(object? obj)
        { AskUserEntryAction?.Invoke(); }
        private bool CanExecuteAskUserEntryCommand(object? obj)
        { return AskUserEntryAction != null; }
        #endregion
    }
}
