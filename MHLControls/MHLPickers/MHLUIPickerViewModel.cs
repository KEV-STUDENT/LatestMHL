using MHLCommands;
using MHLCommon;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

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
            get
            {
                return _view.Value;
            }
            set
            {
                _view.Value = value;
                ValueChangedInform();
            }
        }

        public ICommand AskUserEntryCommand { get; set; }
        public ICommand AskUserSettingsCommand { get; set; }

        public bool IsReadOnlyTextInput
        {
            get => _view.IsReadOnlyTextInput;

            set
            {
                _view.IsReadOnlyTextInput = value;
                IsReadOnlyTextInputChangedInform();
            }
        }

        public int CaptionWidth
        {
            set
            {
                _view.CaptionWidth = value;
                OnPropertyChanged("CaptionWidth");
            }
            get { return _view.CaptionWidth; }
        }

        public string Caption
        {
            set
            {
                _view.Caption = value;
                OnPropertyChanged("Caption");
            }
            get { return _view.Caption; }
        }

        public Visibility CaptionVisibility
        {
            get => _view.CaptionVisibility;

            set
            {
                _view.CaptionVisibility = value;
                OnPropertyChanged("CaptionVisibility");
            }
        }

        public Visibility SettingsVisibility
        {
            get => _view.SettingsVisibility;

            set
            {
                _view.SettingsVisibility = value;
                OnPropertyChanged("SettingsVisibility");
            }
        }
        #endregion

        #region [Constructors]
        public MHLUIPickerViewModel(MHLUIPicker view)
        {
            AskUserEntryCommand = new RelayCommand(ExecuteAskUserEntryCommand, CanExecuteAskUserEntryCommand);
            AskUserSettingsCommand = new RelayCommand(ExecuteAskUserSettingsCommand, CanExecuteAskUserSettingsCommand);
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

        public event Action AskUserSettings
        {
            add { _model.AskUserSettings += value; }
            remove { _model.AskUserSettings -= value; }
        }
        #endregion

        #region [Delegates]
        private Action? ValueChangedAction;
        private Action? IsReadOnlyTextInputChangedAction;
        #endregion

        #region [Private Methods]
        private void ExecuteAskUserEntryCommand(object? obj)
        { _model.AskUserEntryAction(); }
        private bool CanExecuteAskUserEntryCommand(object? obj)
        { return _model.AskUserEntryCanExecute(); }

        private bool CanExecuteAskUserSettingsCommand(object? obj)
        { return _model.AskUserSettingsCanExecute(); }

        private void ExecuteAskUserSettingsCommand(object? obj)
        { _model.AskUserSettingsAction(); }

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

        internal void IsReadOnlyTextInputChangedInform()
        {
            OnPropertyChanged("IsReadOnlyTextInput");
            IsReadOnlyTextInputChangedAction?.Invoke();
        }
        #endregion
    }
}
