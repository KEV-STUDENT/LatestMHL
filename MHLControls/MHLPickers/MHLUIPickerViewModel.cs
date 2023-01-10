using MHLCommands;
using MHLCommon;
using MHLCommon.ViewModels;
using System;
using System.Windows.Input;
using MHLCommon.MHLScanner;

namespace MHLControls.MHLPickers
{
    public class MHLUIPickerViewModel : ViewModel
    {
        #region [Fields]
        private MHLUIPickerModel _model;
        private MHLUIPicker _view;
        #endregion

        #region [Properties]
        public string Value
        {
            get {
                return _view.Value;
            }
            set
            {
                _view.Value = value;
                ValueChangedInform();
            }
        }

        public ICommand AskUserEntryCommand { get; set; }
        #endregion

        #region [Constructors]
        public MHLUIPickerViewModel(MHLUIPicker view)
        {
            AskUserEntryCommand = new RelayCommand(ExecuteAskUserEntryCommand, CanExecuteAskUserEntryCommand);
            _model = new MHLUIPickerModel(this);
            _view = view;           
        }
        #endregion

        #region [Events]
        public event Action ValueChanged
        {
            add { ValueChangedAction += value; }
            remove { ValueChangedAction -= value; }
        }

        public event Action<IPicker<string>> AskUserEntry
        {
            add { _model.AskUserEntry += value; }
            remove { _model.AskUserEntry -= value; }
        }
        #endregion

        #region [Delegates]
        private Action? ValueChangedAction;
        #endregion

        #region [Private Methods]
        private void ExecuteAskUserEntryCommand(object? obj)
        { _model.AskUserEntryAction(); }
        private bool CanExecuteAskUserEntryCommand(object? obj)
        { return _model.AskUserEntryCanExecute(); }

        internal ReturnResultEnum CheckValue(out string value)
        {
            value = Value;
            return ReturnResultEnum.Ok;
        }

        internal void ValueChangedInform()
        {
            OnPropertyChanged("Value");
            ValueChangedAction?.Invoke();
        }
        #endregion
    }
}
