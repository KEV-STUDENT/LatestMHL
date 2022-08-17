using MHLSourceScannerModelLib;
using MHLCommon;
using MHLCommon.MHLScanner;

namespace MHLSourceScannerModelLibTest
{
    [TestClass]
    public class IPickerTest
    {
        [TestMethod]
        public void FilePicker()
        {
            IPicker<string> picker = new FilePickerTest();
            Assert.IsNotNull(picker);
        }


        [TestMethod]
        public void IPickerTest_AskUserForInput_State_AskUserForInput()
        {
            IPicker<String> picker = new FilePickerTest();
            picker.AskUserForInput();

            Assert.AreEqual(TestPickerStateEnum.AskUserForInput, ((FilePickerTest)picker).State[0]);
        }

        [TestMethod]
        public void IPickerTest_CheckValue_State_CheckValue()
        {
            IPicker<string> picker = new FilePickerTest();
            string value;
            ReturnResultEnum result = picker.CheckValue(out value);

            Assert.AreEqual(TestPickerStateEnum.CheckValue, ((FilePickerTest)picker).State[0]);
        }

        [TestMethod]
        public void IPickerTest_CheckValue_State_AskUserForInput()
        {
            IPicker<string> picker = new FilePickerTest();
            string value;
            ReturnResultEnum result = picker.CheckValue(out value);

            Assert.AreEqual(TestPickerStateEnum.AskUserForInput, ((FilePickerTest)picker).State[1]);
        }
    }
}