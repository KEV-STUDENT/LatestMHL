using MHLCommon;
using MHLCommon.MHLScanner;

namespace MHLSourceScannerModelLib
{
    public class DiskItemPicker : IPicker<string>
    {
        string result = String.Empty;
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
            if(AskUserForInputAction != null)
                AskUserForInputAction();
         }

        ReturnResultEnum IPicker<string>.CheckValue(out string value)
        {
            return CheckValue(out value);
        }

        public virtual ReturnResultEnum CheckValue(out string value)
        {
            IPicker<string> picker = this;
           
            if (String.IsNullOrEmpty(picker.Value))
            {
                picker.AskUserForInput();
            }

            value = picker.Value;
            if (String.IsNullOrEmpty(value))
            {
                return ReturnResultEnum.Cancel;
            }
            return ReturnResultEnum.Ok;
        }
    }
}