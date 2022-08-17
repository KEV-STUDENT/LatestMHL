using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHLSourceOnDisk;
using System.Diagnostics;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class IDiskItemDirectoryTest
    {
        protected const string pathDir = @"F:\1\test";
        
        [TestMethod]
        public void IDiskItemDirectory()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);
            Assert.IsInstanceOfType(item, typeof(IDiskItemDirectory));
        }


        [TestMethod]
        public void GetChilds_pathDir()
        {
            IDiskItemDirectory? item = DiskItemFabrick.GetDiskItem(pathDir) as IDiskItemDirectory;
            int cnt = 0;
            if (item != null)
                foreach (var child in item.GetChilds())
                {
                    cnt++;
                    Debug.WriteLine(child.Path2Item);
                }
            Assert.AreNotEqual(0, cnt);
        }

        [TestMethod]
        public void Count_pathDir()
        {
            IDiskCollection? item = DiskItemFabrick.GetDiskItem(pathDir) as IDiskCollection;
            Debug.WriteLine(item?.Count);
            Assert.AreNotEqual(0, item?.Count??0);
        }       
    }
}
