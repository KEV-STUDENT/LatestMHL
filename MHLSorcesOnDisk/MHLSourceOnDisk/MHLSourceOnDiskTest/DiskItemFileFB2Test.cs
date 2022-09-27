using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Compression;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemFileFB2Test
    {
        protected string pathFile = @"F:\1\test\426096.fb2";
        protected string pathZip = @"F:\1\test\fb2-495000-500999.zip";
        protected string pathFile1 = @"F:\1\test\Davydov_Moskovit.454563.fb2";


        [TestMethod]
        public void Constructor_pathFile()
        {
            IDiskItem item = new DiskItemFileFB2(pathFile);
            Assert.IsInstanceOfType(item, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        public void Constructor_pathZip()
        {
            DiskItemFileZip zip = new DiskItemFileZip(pathZip);
            IDiskItem? itemFB2 = null;
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                ZipArchiveEntry? file = zipArchive.GetEntry("495000.fb2");
                if (file != null)
                {
                    itemFB2 = DiskItemFabrick.GetDiskItem(zip, file);
                }
            }
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
        public void Title_pathZip()
        {
            DiskItemFileZip zip = new DiskItemFileZip(pathZip);
            IBook? itemFB2 = null;
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                ZipArchiveEntry? file = zipArchive.GetEntry("495000.fb2");
                if (file != null)
                {
                    itemFB2 = DiskItemFabrick.GetDiskItem(zip, file) as IBook;
                }
            }

            System.Diagnostics.Debug.WriteLine(itemFB2?.Title??string.Empty);
            Assert.AreNotEqual(string.Empty, itemFB2?.Title??string.Empty);
        }

        [TestMethod]
        public void Cover_pathFile()
        {
            IBook item = new DiskItemFileFB2(pathFile1);
            System.Diagnostics.Debug.WriteLine("Title :[" + item.Title + "]");
            System.Diagnostics.Debug.WriteLine("Cover :[" + item.Cover + "]");
            System.Diagnostics.Debug.WriteLine(string.IsNullOrEmpty(item.Cover));
            Assert.AreNotEqual(string.Empty, item.Cover??string.Empty);
        }
    }
}
