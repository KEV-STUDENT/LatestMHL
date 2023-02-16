using MHL_DB_SQLite;
using MHLCommon;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;
using MHLSourceOnDisk.BookDir;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemVirtualGroupTest
    {
        [TestMethod]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip")]
        public void GetChilds_firstGroup(string pathZip)
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
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip")]
        public void GetChilds_firstGroup_Count(string pathZip)
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
        [DataRow(@"F:\1\test\destination\Virtual Group", @"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip", true, "")]
        [DataRow(@"F:\1\test\destination\Virtual Group", @"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip", false, "")]
        [DataRow(@"F:\1\test\destination\Virtual Group", @"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip", false,
          @"{""SubRows"":[{
                    ""SubRows"":[{
                        ""SubRows"":[],
                        ""Items"":[
                            { ""SelectedItemType"":1,""SelectedTypedItemType"":1}],""IsFileName"":true}],
                    ""Items"":[
                            {""SelectedItemType"":2,""SelectedTypedItemType"":0}],""IsFileName"":false}],
                ""Items"":[
                    { ""SelectedItemType"":1,""SelectedTypedItemType"":2}],""IsFileName"":false}")]
        public void ExportBooks(string pathDestination, string pathZip, bool createNewFlag, string jsonStr)
        {
            IDiskItemExported? item = GetFirstVirualGroupFromZip(pathZip) as IDiskItemExported;
            bool result;
            int res = 0, init = 0;


            PathRowDisk? row;
            if (string.IsNullOrEmpty(jsonStr))
                row = null;
            else
                row = JsonSerializer.Deserialize<PathRowDisk>(jsonStr);

            ExpOptions expOptions = new ExpOptions(pathDestination, createNewFlag, row);
            Export2Dir exporter = new Export2Dir(expOptions);

            if (!createNewFlag)
            {
                if (Directory.Exists(pathDestination))
                {
                    init = Directory.GetFiles(pathDestination, "*", SearchOption.AllDirectories).Length;
                }
                init += ((IVirtualGroup?)item)?.Count ?? 0;
            }

            if (createNewFlag)
            {
                Assert.IsTrue(item?.ExportBooks(exporter) ?? false);
            }
            else
            {
                result = item?.ExportBooks(exporter) ?? false;
                if (result)
                    res = Directory.GetFiles(pathDestination, "*", SearchOption.AllDirectories).Length;

                Assert.AreEqual((init == 0 ? -1 : init), res);
            }
        }

        [TestMethod]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-495000-500999.zip"
            , @"F:\1\test\destinationDB\Virtual Group\ExportFB2.SQLite")]
        public void ExportBooks_SQLite(string pathZip, string pathDestination)
        {
            IDiskItemExported? item = GetFirstVirualGroupFromZip(pathZip) as IDiskItemExported;
            int res = 0, init = 0;

            if (File.Exists(pathDestination))
                File.Delete(pathDestination);

            ExpOptions expOptions = new ExpOptions(pathDestination);
            Export2SQLite exporter = new Export2SQLite(expOptions);

            foreach (var vg in ((IDiskCollection)item).GetChilds())
            {
                if (vg is IDiskCollection diskCollection)
                {
                    init += diskCollection.Count;
                }
                else
                    init++;
            }

            if (item.ExportBooks(exporter))
                using (DBModel dB = new DBModelSQLite(pathDestination))
                {
                    res = dB.Books.ToList().Count;
                }

            System.Diagnostics.Debug.WriteLine("{0}-{1}", res, init);
            Assert.AreEqual(init, res);
        }

        private IVirtualGroup? GetFirstVirualGroupFromZip(string path2Zip)
        {
            IDiskCollection? zip = DiskItemFabrick.GetDiskItem(path2Zip) as IDiskCollection;
            IEnumerable<IDiskItem>? childs = zip?.GetChilds();
            return childs?.First() as IVirtualGroup;
        }
    }
}




