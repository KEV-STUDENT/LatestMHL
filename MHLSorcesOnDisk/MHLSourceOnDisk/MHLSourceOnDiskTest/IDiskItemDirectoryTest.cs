using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHLSourceOnDisk;
using System.Diagnostics;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class IDiskItemDirectoryTest
    {
        [TestMethod]
        [DataRow(@"F:\1\test")]
        public void IDiskItemDirectory(string pathDir)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);
            Assert.IsInstanceOfType(item, typeof(IDiskCollection));
        }


        [TestMethod]
        [DataRow(@"F:\1\test")]
        public void GetChilds_pathDir(string pathDir)
        {
            int cnt = 0;
            if (DiskItemFabrick.GetDiskItem(pathDir) is IDiskCollection item)
                foreach (var child in item.GetChilds())
                {
                    cnt++;
                    Debug.WriteLine(child.Path2Item);
                }
            Assert.AreNotEqual(0, cnt);
        }

        [TestMethod]
        [DataRow(@"F:\1\test")]
        public void Count_pathDir(string pathDir)
        {
            IDiskCollection? item = DiskItemFabrick.GetDiskItem(pathDir) as IDiskCollection;
            Debug.WriteLine(item?.Count);
            Assert.AreNotEqual(0, item?.Count??0);
        }

    }
}
