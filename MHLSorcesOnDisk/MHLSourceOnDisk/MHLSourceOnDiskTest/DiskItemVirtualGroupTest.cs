using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHLSourceOnDisk;
using System.Diagnostics;
using MHLCommon;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemVirtualGroupTest
    {
        protected string pathZip = @"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip";

        [TestMethod]
        public void GetChilds_pathZip_firstGroup()
        {
            IDiskCollection? zip = DiskItemFabrick.GetDiskItem(pathZip) as IDiskCollection;
            IEnumerable<IDiskItem> childs = zip.GetChilds();

            IDiskItem? item = null;
            foreach(IDiskItem child in childs)
            {
                item = child;
                break;
            }
              
            Assert.IsInstanceOfType(item, typeof(IDiskItemVirtualGroup));
        }


        [TestMethod]
        public void GetChilds_pathZip_firstGroup_Count()
        {
            IDiskCollection? zip = DiskItemFabrick.GetDiskItem(pathZip) as IDiskCollection;
            IEnumerable<IDiskItem> childs = zip.GetChilds();

            IDiskItemVirtualGroup? item = null;
            foreach (IDiskItem child in childs)
            {
                item = child as IDiskItemVirtualGroup;
                break;
            }

            int cnt = 0;
            foreach(IDiskItem diskItem in item.GetChilds())
            {
                System.Console.WriteLine(diskItem.Name);
                cnt++;
            }
            System.Console.WriteLine(cnt);
            Assert.AreNotEqual<int>(0, cnt);
        }

    }
}




