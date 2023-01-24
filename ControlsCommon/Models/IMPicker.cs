using MHLCommon;
using MHLCommon.MHLScanner;

namespace ControlsCommon.Models
{
    public interface IMPicker
    {
        bool IsReadOnlyTextInput { get; set; }
        ReturnResultEnum CheckValue();
        void AskUserEntryAction();
        bool AskUserEntryCanExecute();
    }

    public interface IMPicker<T> : IMPicker
    {
        T Value { get; set; }
        event Action<IMPicker<T>>? AskUserEntry;
    }
}
