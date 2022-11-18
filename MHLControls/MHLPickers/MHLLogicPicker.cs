using MHLCommon;
using MHLCommon.MHLScanner;
using System;

namespace MHLControls.MHLPickers
{
    public class MHLLogicPicker : IPicker<string>
    {
        private MHLUIPickerViewModel _vm = new MHLUIPickerViewModel();

        #region [Events]
        public event Action<IPicker<string>>? AskUserForInputEvent
        {
            add { PickerViewModel.AskUserEntryAction += ()=>value?.Invoke(this); }
            remove { PickerViewModel.AskUserEntryAction -= ()=>value?.Invoke(this); }
        }
        #endregion

        #region [Properties]
        public MHLUIPickerViewModel PickerViewModel { get => _vm; }
        public string Value
        {
            get { return PickerViewModel.Value; }
            set { PickerViewModel.Value = value; }
        }
        #endregion

        #region [Constructors]
        public MHLLogicPicker()
        {
        }
        #endregion

        #region [IPicker implementation]
        string IPicker<string>.Value
        {
            get { return Value; }
            set { Value = value; }
        }

        ReturnResultEnum IPicker<string>.CheckValue(out string value)
        {
            return CheckValue(out value);
        }

        event Action<IPicker<string>>? IPicker<string>.AskUserForInputEvent
        {
            add{ AskUserForInputEvent += value;}
            remove{ AskUserForInputEvent -= value;}
        }       
        #endregion


        #region[Methods]
        public virtual ReturnResultEnum CheckValue(out string value)
        {
            IPicker<string> picker = this;

            if (string.IsNullOrEmpty(picker.Value))
            {
                PickerViewModel.AskUserEntryAction?.Invoke();
            }

            value = picker.Value;
            if (string.IsNullOrEmpty(value))
            {
                return ReturnResultEnum.Cancel;
            }
            return ReturnResultEnum.Ok;
        }       
        #endregion
    }
}