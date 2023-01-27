using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;
using MHLSourceOnDisk.BookDir;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text.Json;
using MHL_DB_Model;
using MHL_DB_SQLite;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemFileFB2Test
    {
        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Constructor(string pathFile)
        {
            IDiskItem item = new DiskItemFileFB2(pathFile);
            Assert.IsInstanceOfType(item, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2")]
        public void Constructor_ZIP(string pathZip, string fb2Name)
        {
            IMHLBook? itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathZip, fb2Name);
            Assert.IsInstanceOfType(itemFB2, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Constructor_IBook(string pathFile)
        {
            IDiskItem item = new DiskItemFileFB2(pathFile);
            Assert.IsInstanceOfType(item, typeof(IMHLBook));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Title(string pathFile)
        {
            IMHLBook item = new DiskItemFileFB2(pathFile);
            System.Diagnostics.Debug.WriteLine(item.Title);
            Assert.AreNotEqual(string.Empty, item.Title);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "500970.fb2")]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2")]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-060424-074391.zip", "60586.fb2")]
        public void Title_ZIP(string pathZip, string fb2Name)
        {
            IMHLBook? itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathZip, fb2Name);
            System.Diagnostics.Debug.WriteLine(itemFB2?.Title ?? "NULL");
            Assert.AreNotEqual(string.Empty, itemFB2?.Title ?? string.Empty);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2")]
        public void Cover(string pathFile)
        {
            IMHLBook item = new DiskItemFileFB2(pathFile);
            System.Diagnostics.Debug.WriteLine("Title :[" + item.Title + "]");
            System.Diagnostics.Debug.WriteLine("Cover :[" + item.Cover + "]");
            System.Diagnostics.Debug.WriteLine(string.IsNullOrEmpty(item.Cover));
            Assert.AreNotEqual(string.Empty, item.Cover ?? string.Empty);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\0000\495008.fb2")]
        [DataRow(@"F:\1\test\0000\500985.fb2")]
        public void Sequence_Name(string pathFile)
        {
            IMHLBook item = new DiskItemFileFB2(pathFile);
            System.Diagnostics.Debug.WriteLine(pathFile);
            System.Diagnostics.Debug.WriteLine("Title :[" + item.Title + "]");

            MHLSequenceNum? sequenceNum = item.SequenceAndNumber.First() as MHLSequenceNum;

            System.Diagnostics.Debug.WriteLine("Sequence Name :[" + (sequenceNum?.Name ?? string.Empty) + "]");
            System.Diagnostics.Debug.WriteLine("Sequence Number :[" + (sequenceNum?.Number ?? 0).ToString() + "]");
            Assert.AreNotEqual(string.Empty, sequenceNum?.Name ?? string.Empty);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destination\FB2Test", true, "")]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destination\FB2Test", false, "")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destination", true, "")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destination", false, "")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destination", false,
            @"{""SubRows"":[{
                    ""SubRows"":[{
                        ""SubRows"":[],
                        ""Items"":[
                            { ""SelectedItemType"":1,""SelectedTypedItemType"":1}],""IsFileName"":true}],
                    ""Items"":[
                            {""SelectedItemType"":2,""SelectedTypedItemType"":0},
                            { ""SelectedItemType"":3,""SelectedTypedItemType"":0}],""IsFileName"":false}],
                ""Items"":[
                    {""SelectedItemType"":2,""SelectedTypedItemType"":0},
                    { ""SelectedItemType"":3,""SelectedTypedItemType"":0},
                    { ""SelectedItemType"":1,""SelectedTypedItemType"":3}],""IsFileName"":false}")]
        public void ExportBooks(string pathFile, string fb2Name, string pathDestination, bool overWriteFile, string jsonStr)
        {
            bool result = false;
            int res = 0, init = 0;
            DiskItemFileFB2? itemFB2;


            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            if (itemFB2 != null)
            {
                PathRowDisk? row;
                if (string.IsNullOrEmpty(jsonStr))
                    row = null;
                else
                    row = JsonSerializer.Deserialize<PathRowDisk>(jsonStr);

                ExpOptions expOptions = new ExpOptions(pathDestination, overWriteFile, row);
                Export2Dir exporter = new Export2Dir(expOptions, itemFB2);
                pathDestination = exporter.GetDestinationDir(itemFB2);

                if (!overWriteFile)
                {
                    if (Directory.Exists(pathDestination))
                    {
                        init = Directory.GetFiles(pathDestination).Length;
                    }
                    init += 1;
                }

                result = itemFB2.ExportBooks(exporter);
            }

            if (overWriteFile)
                Assert.IsTrue(result && File.Exists(Path.Combine(pathDestination, itemFB2?.Name ?? "Name == NULL")));
            else
            {
                if (result)
                {
                    res = Directory.GetFiles(pathDestination).Length;
                }
                Assert.AreEqual(init, res);
            }
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        public void ExportBooks2SQLite_Genres(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int cnt = 0;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            ExpOptions expOptions = new ExpOptions(fileSQlite);
            Export2SQLite exporter = new Export2SQLite(expOptions);

            result = itemFB2?.ExportBooks(exporter) ?? false;
            result = result && File.Exists(fileSQlite);
            if (result)
            {
                using (DBModel dB = new DBModel(fileSQlite))
                {
                    cnt = dB.Genres.ToList().Count;
                }
            }
            Assert.AreNotEqual(0, (result ? cnt : 0));
        }
    }
}
