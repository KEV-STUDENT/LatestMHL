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
        protected string pathFile = @"F:\1\test\426096.fb2";

        protected string pathZip = @"F:\1\test\fb2-495000-500999.zip";
        protected string fb2Name = "495000.fb2";

        protected string pathFile1 = @"F:\1\test\Davydov_Moskovit.454563.fb2";
        protected string pathFile2 = @"F:\1\test\0000\495008.fb2";
        protected string pathFile3 = @"F:\1\test\0000\500985.fb2";

        protected string pathDestination = @"F:\1\test\destination\FB2Test";

        [TestMethod]
        public void Constructor_pathFile()
        {
            IDiskItem item = new DiskItemFileFB2(pathFile);
            Assert.IsInstanceOfType(item, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        public void Constructor_pathZip_fb2Name()
        {
            IBook? itemFB2 = GetBookFromZip(pathZip, fb2Name);
            Assert.IsInstanceOfType(itemFB2, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        public void Constructor_pathFile_IBook()
        {
            IDiskItem item = new DiskItemFileFB2(pathFile);
            Assert.IsInstanceOfType(item, typeof(IBook));
        }

        [TestMethod]
        public void Title_pathFile()
        {
            IBook item = new DiskItemFileFB2(pathFile);
            System.Diagnostics.Debug.WriteLine(item.Title);
            Assert.AreNotEqual(string.Empty, item.Title);
        }

        [TestMethod]
        public void Title_pathZip_fb2Name()
        {
            IBook? itemFB2 = GetBookFromZip(pathZip, fb2Name);
            System.Diagnostics.Debug.WriteLine(itemFB2?.Title ?? string.Empty);
            Assert.AreNotEqual(string.Empty, itemFB2?.Title ?? string.Empty);
        }

        [TestMethod]
        public void Cover_pathFile()
        {
            IBook item = new DiskItemFileFB2(pathFile1);
            System.Diagnostics.Debug.WriteLine("Title :[" + item.Title + "]");
            System.Diagnostics.Debug.WriteLine("Cover :[" + item.Cover + "]");
            System.Diagnostics.Debug.WriteLine(string.IsNullOrEmpty(item.Cover));
            Assert.AreNotEqual(string.Empty, item.Cover ?? string.Empty);
        }
        [TestMethod]
        public void Sequence_Name_pathFile2()
        {
            System.Diagnostics.Debug.WriteLine(pathFile2);
            IBook item = new DiskItemFileFB2(pathFile2);
            System.Diagnostics.Debug.WriteLine("Title :[" + item.Title + "]");
            MHLSequenceNum? sequenceNum = item.SequenceAndNumber.First() as MHLSequenceNum;

            System.Diagnostics.Debug.WriteLine("Sequence Name :[" + (sequenceNum?.Name ?? string.Empty) + "]");
            System.Diagnostics.Debug.WriteLine("Sequence Number :[" + (sequenceNum?.Number ?? 0).ToString() + "]");
            Assert.AreNotEqual(string.Empty, sequenceNum?.Name ?? string.Empty);
        }

        [TestMethod]
        public void Sequence_Name_pathFile3()
        {
            IBook item = new DiskItemFileFB2(pathFile3);
            System.Diagnostics.Debug.WriteLine(pathFile3);
            System.Diagnostics.Debug.WriteLine("Title :[" + item.Title + "]");

            MHLSequenceNum? sequenceNum = item.SequenceAndNumber.First() as MHLSequenceNum;

            System.Diagnostics.Debug.WriteLine("Sequence Name :[" + (sequenceNum?.Name ?? string.Empty) + "]");
            System.Diagnostics.Debug.WriteLine("Sequence Number :[" + (sequenceNum?.Number ?? 0).ToString() + "]");
            Assert.AreNotEqual(string.Empty, sequenceNum?.Name ?? string.Empty);
        }

        [TestMethod]
        public void ExportBooks_pathZip_fb2Name_pathDestination_false()
        {
            bool result = false;
            int res = 0, init = 0;

            if (Directory.Exists(pathDestination))
            {
                init = Directory.GetFiles(pathDestination).Length;
            }
            init += 1;

            DiskItemFileFB2? itemFB2 = GetBookFromZip(pathZip, fb2Name) as DiskItemFileFB2;
            if (itemFB2 != null)
            {
                ExpOptions expOptions = new ExpOptions(pathDestination, false);
                Export2Dir exporter = new Export2Dir(expOptions);
                result = itemFB2.ExportBooks(exporter);
            }

            if (result)
            {
                res = Directory.GetFiles(pathDestination).Length;
            }
            Assert.AreEqual(init, res);
        }

        [TestMethod]
        public void ExportBooks_pathZip_fb2Name_pathDestination_true()
        {
            bool result = false;
            DiskItemFileFB2? itemFB2 = GetBookFromZip(pathZip, fb2Name) as DiskItemFileFB2;
            if (itemFB2 != null)
            {
                ExpOptions expOptions = new ExpOptions(pathDestination, true);
                Export2Dir exporter = new Export2Dir(expOptions);
                result = itemFB2.ExportBooks(exporter);
            }
            Assert.IsTrue(result && File.Exists(Path.Combine(pathDestination, fb2Name)));
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
