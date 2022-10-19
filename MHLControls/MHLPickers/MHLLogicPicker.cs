using System;
using MHLCommon;
using MHLCommon.MHLScanner;

namespace MHLControls.MHLPickers
{
    public class MHLLogicPicker : IPicker<string>
    {
        string result = string.Empty;
        public Action? AskUserForInputAction;

        string IPicker<string>.Value
        {
            get { return result; }
            set { result = value; }
        }

        void IPicker<string>.AskUserForInput()
        {
            AskUserForInput();
        }

        public virtual void AskUserForInput()
        {
            if (AskUserForInputAction != null)
                AskUserForInputAction();
        }

        ReturnResultEnum IPicker<string>.CheckValue(out string value)
        {
            return CheckValue(out value);
        }

        public virtual ReturnResultEnum CheckValue(out string value)
        {
            IPicker<string> picker = this;

            if (string.IsNullOrEmpty(picker.Value))
            {
                picker.AskUserForInput();
            }

            value = picker.Value;
            if (string.IsNullOrEmpty(value))
            {
                return ReturnResultEnum.Cancel;
            }
            return ReturnResultEnum.Ok;
        }
    }
}