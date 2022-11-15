using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHLSourceOnDisk;
using System.Diagnostics;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class IDiskItemTest
    {
        [TestMethod]
        [DataRow(@"F:\1\test")]
        public void GetDiskItem(string pathDir)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);
            Debug.WriteLine(item.Path2Item);

            Assert.AreSame(item.Path2Item, pathDir);
        }

        [TestMethod]
        [DataRow(@"F:\1\test")]
        public void Name(string pathDir)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);
            Debug.WriteLine(item.Name);

            Assert.AreEqual(item.Name, "test");
        }
    }
}
