using ControlsCommon.ControlsViews;
using MHLCommon.ViewModels;
using System.Windows.Input;

namespace ControlsCommon.ViewModels
{
    public class VMPicker<T> : ViewModel, IVMPicker<T>
    {
        private IPickerView view;

        #region [Constructors]
        public VMPicker(IPickerView viewUI)
        {
            this.view = viewUI;
        }

        #region [IVMPicker]
        ICommand IVMPicker.AskUserEntryCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        void IVMPicker.ExecuteAskUserEntryCommand(object? obj)
        { _model.AskUserEntryAction(); }
        bool IVMPicker.CanExecuteAskUserEntryCommand(object? obj)
        { return _model.AskUserEntryCanExecute(); }
        #region
    }
}