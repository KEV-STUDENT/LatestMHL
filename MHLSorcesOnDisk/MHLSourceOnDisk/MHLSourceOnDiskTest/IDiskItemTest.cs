using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHLSourceOnDisk;
using System.Diagnostics;
using MHLCommon;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class IDiskItemTest
    {
        protected string pathDir = @"F:\1\test";
        protected string pathFile = @"F:\1\test\426096.fb2";
        protected string pathError = @"F1:\1\test\426096.fb2";

        [TestMethod]
        public void IDiskItem_GetDiskItem_pathDir()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);
            Debug.WriteLine(item.Path2Item);

            Assert.AreSame(item.Path2Item, pathDir);
        }

        [TestMethod]
        public void IDiskItem_Name_pathDir()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);
            Debug.WriteLine(item.Name);

            Assert.AreEqual(item.Name, "test");
        }
    }
}
