using ControlsCommon.ViewModels.Pickers;
using MHLCommon;
using MHLCommon.MHLScanner;

namespace ControlsCommon.Models
{
    public class MPicker<T> : IMPicker<T>
    {
        #region [Fields]
        IVMPicker<T> _vm;
        #endregion

        #region [Delegates]
        private Action<IMPicker<T>>? askUserEntry;
        #endregion

        #region [Constructors]
        public MPicker(IVMPicker<T> vm)
        {
            _vm = vm;
        }
        #endregion

        #region [Methods]
        public void AskUserEntryAction()
        {
            askUserEntry?.Invoke(this);
        }
        public bool AskUserEntryCanExecute()
        {
            return askUserEntry != null;
        }
        #endregion

        #region [IMPicker]
        bool IMPicker.IsReadOnlyTextInput { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        T IMPicker<T>.Value { get => _vm.Value; set => _vm.Value = value; }
      

        event Action<IMPicker<T>>? IMPicker<T>.AskUserEntry
        {
            add
            {
                askUserEntry+=value;
            }

            remove
            {
                askUserEntry-=value;
            }
        }

        void IMPicker.AskUserEntryAction()
        {
            AskUserEntryAction();
        }

        bool IMPicker.AskUserEntryCanExecute()
        {
            return AskUserEntryCanExecute();
        }

        ReturnResultEnum IMPicker.CheckValue()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
