using MHLSourceScannerModelLib;
using System.Diagnostics;
using MHLCommon;


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
            Debug.WriteLine(shower.SourceItems[0].Path2Item);
        }

    }
}