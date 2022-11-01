using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHLSourceOnDisk;
using MHLSourceScannerModelLib;
using System.Collections.Generic;
using MHLCommon.MHLDiskItems;
using MHLCommon;
using System.IO;
using System.IO.Compression;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemFileZipTest
    {
        protected string pathZip1 = @"F:\1\test\fb2-495000-500999.zip";
        protected string pathZip = @"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip";
        protected int pathZipCnt = 30;

        protected string pathDestination = @"F:\1\test\destination";

        [TestMethod]
        public void GetChilds_pathDir_IDiskItem()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathZip);
            int cnt = 0;
            if (item is IDiskCollection zip)
            {
                IEnumerable<IDiskItem> childs = zip.GetChilds();
                foreach (IDiskItem child in childs)
                {
                    System.Diagnostics.Debug.WriteLine(child.Name);
                    cnt++;
                }
            }
            Assert.AreNotEqual(0, cnt);
        }

      
        [TestMethod]
        public void Count_pathZip()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathZip);
            IDiskCollection? diskCollection = item as IDiskCollection;
            System.Diagnostics.Debug.WriteLine(diskCollection?.Count);
            Assert.AreNotEqual(0, diskCollection?.Count??0);
        }

        [TestMethod]
        public void GetChilds_IDiskVirtualGroup_pathZip()
        {
            IDiskCollection? zip = DiskItemFabrick.GetDiskItem(pathZip) as IDiskCollection;
            IEnumerable<IDiskItem> childs = zip.GetChilds();
            
            System.Diagnostics.Debug.WriteLine(zip?.Count);
            IDiskItem? childFirst = null;

            foreach (IDiskItem child in childs)
            {
                childFirst = child;
                break;
            }
            Assert.IsInstanceOfType(childFirst, typeof(DiskItemVirtualGroup));
        }

        [TestMethod]
        public void GetChilds_Count_pathZip_pathZipCnt()
        {
            IDiskCollection? zip = DiskItemFabrick.GetDiskItem(pathZip) as IDiskCollection;
            IEnumerable<IDiskItem> childs = zip.GetChilds();

            System.Diagnostics.Debug.WriteLine(zip?.Count);

            int cnt = 0;
            foreach (IDiskItem child in childs)
            {
                cnt++;
                System.Diagnostics.Debug.WriteLine(child.Name);
            }
            Assert.AreEqual(pathZipCnt, cnt);
        }

        [TestMethod]
        public void ExportBooks_pathZip_true_pathDestination()
        {
            IDiskItem zip = DiskItemFabrick.GetDiskItem(pathZip);
            Directory.Delete(pathDestination, true);

            ExpOptions expOptions = new ExpOptions(pathDestination, true);
            Export2Dir exporter = new Export2Dir(expOptions);
            Assert.IsTrue( zip.ExportBooks(exporter));
        }

        [TestMethod]
        public void ExportBooks_pathZip_false_pathDestination()
        {
            IDiskItem zip = DiskItemFabrick.GetDiskItem(pathZip);
            int res = 0, init = 0;
            if(Directory.Exists(pathDestination))
            {
                init = Directory.GetFiles(pathDestination).Length;
            }    
            ExpOptions expOptions = new ExpOptions(pathDestination, false);
            Export2Dir exporter = new Export2Dir(expOptions);

            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                init += zipArchive.Entries.Count;
            }

            if (zip.ExportBooks(exporter) && Directory.Exists(pathDestination))
            {
                res = Directory.GetFiles(pathDestination).Length;
            }

            System.Diagnostics.Debug.WriteLine("{0}-{1}", res, init);
            Assert.AreEqual(init, res);
        }
    }
}
