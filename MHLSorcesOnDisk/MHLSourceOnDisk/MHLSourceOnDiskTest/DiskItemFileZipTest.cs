using MHLCommon;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;
using MHLSourceOnDisk.BookDir;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemFileZipTest
    {
        [TestMethod]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip")]
        public void GetChilds_IDiskItem(string pathZip)
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
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip")]
        public void Count(string pathZip)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathZip);
            IDiskCollection? diskCollection = item as IDiskCollection;
            System.Diagnostics.Debug.WriteLine(diskCollection?.Count);
            Assert.AreNotEqual(0, diskCollection?.Count ?? 0);
        }

        [TestMethod]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip")]
        public void GetChilds_IDiskVirtualGroup(string pathZip)
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
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip", 30)]
        public void GetChilds_Count(string pathZip, int pathZipCnt)
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
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip", @"F:\1\test\destination", true, "")]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip", @"F:\1\test\destination", false, "")]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip", @"F:\1\test\destination", true,
            @"{""SubRows"":[{
                    ""SubRows"":[{
                        ""SubRows"":[],
                        ""Items"":[
                            { ""SelectedItemType"":1,""SelectedTypedItemType"":1}],""IsFileName"":true}],
                    ""Items"":[
                            {""SelectedItemType"":2,""SelectedTypedItemType"":0}],""IsFileName"":false}],
                ""Items"":[
                    { ""SelectedItemType"":1,""SelectedTypedItemType"":2}],""IsFileName"":false}")]
        public void ExportBooks(string pathZip, string pathDestination, bool createNewFlag, string jsonStr)
        {
            IDiskItem zip = DiskItemFabrick.GetDiskItem(pathZip);
            int res = 0, init = 0;

            if (createNewFlag)
            {
                Directory.Delete(pathDestination, true);
            }
            else
            {
                if (Directory.Exists(pathDestination))
                {
                    init = Directory.GetFiles(pathDestination,"*", SearchOption.AllDirectories).Length;
                }
            }

            PathRowDisk? row;
            if (string.IsNullOrEmpty(jsonStr))
                row = null;
            else
                row = JsonSerializer.Deserialize<PathRowDisk>(jsonStr);

            ExpOptions expOptions = new ExpOptions(pathDestination, createNewFlag, row);
            Export2Dir exporter = new Export2Dir(expOptions);

            if (createNewFlag)
                Assert.IsTrue( zip.ExportBooks(exporter));
            else
            {
                foreach(var vg in ((IDiskCollection)zip).GetChilds())
                {
                    if (vg is IDiskCollection diskCollection)
                    {
                        init += diskCollection.Count;
                    }
                    else
                        init++;
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
}
