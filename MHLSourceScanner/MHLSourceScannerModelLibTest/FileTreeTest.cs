using MHLSourceScannerModelLib;
using System.Diagnostics;
using MHLCommon.MHLScanner;

namespace MHLSourceScannerModelLibTest
{
    internal class FileTreeTest : DiskItemShower
    {
       public FileTreeTest()
        {
            UpdateView = UpdateViewAction;
        }

        private void UpdateViewAction()
        {
            IShower shower = this;
        }
    }
}