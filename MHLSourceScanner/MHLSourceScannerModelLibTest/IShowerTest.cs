using MHLSourceScannerModelLib;
using MHLCommon;


namespace MHLSourceScannerModelLibTest
{
    [TestClass]
    public class IShowerTest
    {
        protected readonly string pathDir = @"F:\1\test";
        protected readonly string pathDirZip = @"E:\librus_MyHomeLib\lib.rus.ec\fb2-284000-291999.zip";

        [TestMethod]
        public void FileTree()
        {
            IShower tree = new FileTreeTest();
            Assert.IsNotNull(tree);
        }        
    } 
}
