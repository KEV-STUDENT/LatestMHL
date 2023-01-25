using ControlsCommon.ControlsViews;
using ControlsCommon.Models;
using MHLCommands;
using MHLCommon.ViewModels;
using System.Windows.Input;

namespace ControlsCommon.ViewModels.Pickers
{
    public class VMPicker<T> : ViewModel, IVMPicker<T>
    {
        #region [Fields]
        private IPickerView<T> _view;
        private IMPicker<T> _model;
        #endregion

        #region [Delegates]
        private Action? _valueChanged;
        #endregion

        #region [Constructors]
        public VMPicker(IPickerView<T> viewUI)
        {
            _view = viewUI;           
            _valueChanged = _view.ValueChanged;
            AskUserEntryCommand = new RelayCommand(ExecuteAskUserEntryCommand, CanExecuteAskUserEntryCommand);
            InitVM();
        }
        #endregion

        #region [Methods]
        protected virtual void InitVM()
        {
            _model = new MPicker<T>(this);
        }

        private void ValueChangedInform()
        {
            OnPropertyChanged("Value");
            _valueChanged?.Invoke();
        }

        private void ExecuteAskUserEntryCommand(object? obj)
        { _model.AskUserEntryAction(); }
        private bool CanExecuteAskUserEntryCommand(object? obj)
        { return _model.AskUserEntryCanExecute(); }
        #endregion

        #region [Properties]
        public ICommand AskUserEntryCommand { get; set; }
        #endregion

        #region [IVMPicker]
        ICommand IVMPicker<T>.AskUserEntryCommand {get => AskUserEntryCommand; set => AskUserEntryCommand = value; }
        T IVMPicker<T>.Value
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

        event Action<IMPicker<T>> IVMPicker<T>.AskUserEntry
        {
            add
            {
                _model.AskUserEntry += value;
            }

            remove
            {
                _model.AskUserEntry -= value;
            }
        }

        event Action IVMPicker.ValueChanged
        {
            add { _valueChanged += value; }
            remove { _valueChanged -= value; }
        }

        void IVMPicker.ExecuteAskUserEntryCommand(object? obj)
        { ExecuteAskUserEntryCommand(obj); }
        bool IVMPicker.CanExecuteAskUserEntryCommand(object? obj)
        { return CanExecuteAskUserEntryCommand(obj); }

        void IVMPicker.ValueChangedInform()
        {
            ValueChangedInform();
        }

        void IVMPicker.IsReadOnlyTextInputChangedInform()
        {
            OnPropertyChanged("IsReadOnlyTextInput");
        }
        #endregion
    }
}