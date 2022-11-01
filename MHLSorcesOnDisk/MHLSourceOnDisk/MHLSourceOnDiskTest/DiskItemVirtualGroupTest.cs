using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHLSourceOnDisk;
using System.Diagnostics;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemVirtualGroupTest
    {
        protected string pathZip = @"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip";
        protected string pathDestination = @"F:\1\test\destination";

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
              
            Assert.IsInstanceOfType(item, typeof(IVirtualGroup));
        }


        [TestMethod]
        public void GetChilds_pathZip_firstGroup_Count()
        {
            IDiskCollection? zip = DiskItemFabrick.GetDiskItem(pathZip) as IDiskCollection;
            IEnumerable<IDiskItem> childs = zip.GetChilds();

            IVirtualGroup? item = null;
            foreach (IDiskItem child in childs)
            {
                item = child as IVirtualGroup;
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

        [TestMethod]
        public void ExportBooks_pathZip_pathDestination()
        {
            IDiskCollection zip = DiskItemFabrick.GetDiskItem(pathZip) as IDiskCollection;
            IEnumerable<IDiskItem> childs = zip.GetChilds();
            IDiskItem item = childs.First();

            if(System.IO.Directory.Exists(pathDestination))
               System.IO.Directory.Delete(pathDestination, true);

            Export2Dir exporter = new Export2Dir(pathDestination);


            System.Diagnostics.Debug.WriteLine("{0} : {1}", item.Path2Item, item.Name);
            Assert.IsTrue(item.ExportBooks(exporter));
        }
    }
}




