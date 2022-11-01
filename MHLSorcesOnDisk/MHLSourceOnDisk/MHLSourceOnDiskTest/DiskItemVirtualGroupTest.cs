using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHLSourceOnDisk;
using System.Diagnostics;
using MHLCommon.MHLDiskItems;
using MHLCommon;
using System.IO;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemVirtualGroupTest
    {
        protected string pathZip = @"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip";
        protected string pathDestination = @"F:\1\test\destination\Virtual Group";

        [TestMethod]
        public void GetChilds_pathZip_firstGroup()
        {
            IDiskCollection? zip = DiskItemFabrick.GetDiskItem(pathZip) as IDiskCollection;
            IEnumerable<IDiskItem> childs = zip.GetChilds();

            IDiskItem? item = null;
            foreach (IDiskItem child in childs)
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
            foreach (IDiskItem diskItem in item.GetChilds())
            {
                System.Console.WriteLine(diskItem.Name);
                cnt++;
            }
            System.Console.WriteLine(cnt);
            Assert.AreNotEqual<int>(0, cnt);
        }

        [TestMethod]
        public void ExportBooks_pathZip_pathDestination_true()
        {
            IDiskItem? item = GetFirstVirualGroupFromZip(pathZip) as IDiskItem;

            ExpOptions expOptions = new ExpOptions(pathDestination, true);
            Export2Dir exporter = new Export2Dir(expOptions);

            System.Diagnostics.Debug.WriteLine("{0} : {1}", item?.Path2Item ?? string.Empty, item?.Name ?? string.Empty);
            Assert.IsTrue(item?.ExportBooks(exporter) ?? false);
        }

        [TestMethod]
        public void ExportBooks_pathZip_pathDestination_false()
        {
            IDiskItem? item = GetFirstVirualGroupFromZip(pathZip) as IDiskItem;
            bool result = false;
            int res = 0, init = 0;
            if (Directory.Exists(pathDestination))
            {
                init = Directory.GetFiles(pathDestination).Length;
            }
            init += ((IVirtualGroup?)item)?.Count ?? 0;

            ExpOptions expOptions = new ExpOptions(pathDestination, false);
            Export2Dir exporter = new Export2Dir(expOptions);

            System.Diagnostics.Debug.WriteLine("{0} : {1}", item?.Path2Item ?? string.Empty, item?.Name ?? string.Empty);
            result = item?.ExportBooks(exporter) ?? false;

            if (result)
            {
                res = Directory.GetFiles(pathDestination).Length;
            }

            Assert.AreEqual((init == 0 ? -1 : init), res);
        }

        private IVirtualGroup? GetFirstVirualGroupFromZip(string path2Zip)
        {
            IDiskCollection? zip = DiskItemFabrick.GetDiskItem(path2Zip) as IDiskCollection;
            IEnumerable<IDiskItem>? childs = zip?.GetChilds();
            return childs?.First() as IVirtualGroup;
        }
    }
}




