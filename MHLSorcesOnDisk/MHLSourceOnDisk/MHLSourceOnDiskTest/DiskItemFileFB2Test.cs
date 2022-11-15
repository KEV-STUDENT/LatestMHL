using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.Compression;
using System.Linq;

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
            IBook? itemFB2 = GetBookFromZip(pathZip, fb2Name);
            Assert.IsInstanceOfType(itemFB2, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Constructor_IBook(string pathFile)
        {
            IDiskItem item = new DiskItemFileFB2(pathFile);
            Assert.IsInstanceOfType(item, typeof(IBook));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Title(string pathFile)
        {
            IBook item = new DiskItemFileFB2(pathFile);
            System.Diagnostics.Debug.WriteLine(item.Title);
            Assert.AreNotEqual(string.Empty, item.Title);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "500970.fb2")]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2")]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-060424-074391.zip", "60586.fb2")]
        public void Title_ZIP(string pathZip, string fb2Name)
        {
            IBook? itemFB2 = GetBookFromZip(pathZip, fb2Name);
            System.Diagnostics.Debug.WriteLine(itemFB2?.Title ?? "NULL");
            Assert.AreNotEqual(string.Empty, itemFB2?.Title ?? string.Empty);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2")]
        public void Cover(string pathFile)
        {
            IBook item = new DiskItemFileFB2(pathFile);
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
            IBook item = new DiskItemFileFB2(pathFile);
            System.Diagnostics.Debug.WriteLine(pathFile);
            System.Diagnostics.Debug.WriteLine("Title :[" + item.Title + "]");

            MHLSequenceNum? sequenceNum = item.SequenceAndNumber.First() as MHLSequenceNum;

            System.Diagnostics.Debug.WriteLine("Sequence Name :[" + (sequenceNum?.Name ?? string.Empty) + "]");
            System.Diagnostics.Debug.WriteLine("Sequence Number :[" + (sequenceNum?.Number ?? 0).ToString() + "]");
            Assert.AreNotEqual(string.Empty, sequenceNum?.Name ?? string.Empty);
        }


        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destination\FB2Test", true)]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destination\FB2Test", false)]
        public void ExportBooks(string pathZip, string fb2Name, string pathDestination, bool createNewFlag)
        {
            bool result = false;
            int res = 0, init = 0;

            if(!createNewFlag)
            {
                if (Directory.Exists(pathDestination))
                {
                    init = Directory.GetFiles(pathDestination).Length;
                }
                init += 1;
            }

            DiskItemFileFB2? itemFB2 = GetBookFromZip(pathZip, fb2Name) as DiskItemFileFB2;
            if (itemFB2 != null)
            {
                ExpOptions expOptions = new ExpOptions(pathDestination, createNewFlag);
                Export2Dir exporter = new Export2Dir(expOptions);
                result = itemFB2.ExportBooks(exporter);
            }
            if (createNewFlag)
                Assert.IsTrue(result && File.Exists(Path.Combine(pathDestination, fb2Name)));
            else
            {
                if (result)
                {
                    res = Directory.GetFiles(pathDestination).Length;
                }
                Assert.AreEqual(init, res);
            }
        }

        private IBook? GetBookFromZip(string pathZip, string fb2Name)
        {
            DiskItemFileZip zip = new DiskItemFileZip(pathZip);
            IBook? itemFB2 = null;
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                ZipArchiveEntry? file = zipArchive.GetEntry(fb2Name);
                if (file != null)
                {
                    itemFB2 = DiskItemFabrick.GetDiskItem(zip, file) as IBook;
                }
            }

            return itemFB2;
        }
    }
}
