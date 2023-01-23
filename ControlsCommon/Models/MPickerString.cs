using MHLCommon;
using MHLCommon.MHLScanner;

namespace ControlsCommon.Models
{
    public class MPickerString : IMPicker<string>
    {
        #region [IMPicker]
        string IMPicker<string>.Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMPicker<string>.IsReadOnlyTextInput { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        event Action<IPicker<string>>? IMPicker<string>.AskUserEntry
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        ReturnResultEnum IMPicker<string>.CheckValue()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
