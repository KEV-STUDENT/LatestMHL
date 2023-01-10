using MHLCommon;
using MHLCommon.MHLScanner;
using System;

namespace MHLControls.MHLPickers
{
    internal class MHLUIPickerModel : IPicker<String>
    {
        #region [Delegates]
        private Action<IPicker<string>>? askUserEntry;
        #endregion

        #region [Fields]
        private readonly  MHLUIPickerViewModel _vm;
        #endregion

        #region [Properties]
        protected string Value
        {
            get => _vm.Value;
            set => _vm.Value = value;
        }
        #endregion

        #region [Constructors]
        public MHLUIPickerModel(MHLUIPickerViewModel vm)
        {
            _vm = vm;
        }
        #endregion

        #region [Events]
        public event Action<IPicker<string>>? AskUserEntry 
        {
            add { 
                askUserEntry += value;
            }
            remove { 
                askUserEntry -= value;
            }
        }       
        #endregion

        #region [Methods]
        public void AskUserEntryAction()
        {
            askUserEntry?.Invoke(this);
        }

        internal bool AskUserEntryCanExecute()
        {
            return askUserEntry != null;
        }
        #endregion

        #region [IPicker implementation]
        string IPicker<string>.Value { 
            get => Value; 
            set => Value = value; 
        }

        event Action<IPicker<string>>? IPicker<string>.AskUserEntry
        {
            add{ AskUserEntry += value;}
            remove{ AskUserEntry -= value;}
        }

        ReturnResultEnum IPicker<string>.CheckValue()
        {
            AskUserEntryAction();
            if (string.IsNullOrEmpty(Value))
            {
                return ReturnResultEnum.Cancel;
            }
            return ReturnResultEnum.Ok;
        }
        #endregion
    }
}
