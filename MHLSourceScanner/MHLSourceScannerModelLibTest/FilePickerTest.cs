using MHLSourceScannerModelLib;
using MHLCommon;

namespace MHLSourceScannerModelLibTest
{
    internal class FilePickerTest : DiskItemPicker
    {
        List<TestPickerStateEnum> state = new List<TestPickerStateEnum>();

        public List<TestPickerStateEnum> State { get => state; }

        public override void AskUserForInput() 
        {
            state.Add(TestPickerStateEnum.AskUserForInput);
        }

        public override ReturnResultEnum CheckValue(out string value)
        {
            state.Add(TestPickerStateEnum.CheckValue);
            return base.CheckValue(out value);
        }
    }
}