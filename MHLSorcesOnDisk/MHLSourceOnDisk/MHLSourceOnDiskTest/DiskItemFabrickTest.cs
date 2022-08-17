using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Compression;
using MHLSourceOnDisk;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemFabrickTest
    {
        protected string pathDir = @"F:\1\test";
        protected string pathFile = @"F:\1\test\426096.fb2";
        protected string pathError = @"F1:\1\test\426096.fb2";
        protected string pathZip = @"F:\1\test\fb2-495000-500999.zip";
        protected const string pathAccess = @"C:\Config.Msi";

        [TestMethod]
        public void GetDiskItem_pathDir_IDiskItem()
        {            
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);

            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetDiskItem_pathDir_IDiskItemDirectory()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);
            Assert.IsInstanceOfType(item, typeof(IDiskItemDirectory));
        }

        [TestMethod]
        public void GetDiskItem_pathFile_IDiskItemFile()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathFile);
            Assert.IsInstanceOfType(item, typeof(IDiskItemFile));
        }

        [TestMethod]
        public void GetDiskItem_pathFile_DiskItemFileFB2()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathFile);
            Assert.IsInstanceOfType(item, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        public void GetDiskItem_pathError_IDiskItemError()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathError);
            Assert.IsInstanceOfType(item, typeof(IDiskItemError));
        }

        [TestMethod]
        public void GetDiskItem_pathZip_DiskItemFileZip()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathZip);
            Assert.IsInstanceOfType(item, typeof(DiskItemFileZip));
        }

        [TestMethod]
        public void GetDiskItem_pathDir_IDiskCollection()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);
            Assert.IsInstanceOfType(item, typeof(IDiskCollection));
        }

        [TestMethod]
        public void GetDiskItem_pathZip_IDiskCollection()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathZip);
            Assert.IsInstanceOfType(item, typeof(IDiskCollection));
        }

        [TestMethod]
        public void GetDiskItem_pathZip_1_NotDiskItemError()
        {
            DiskItemFileZip? zip = DiskItemFabrick.GetDiskItem(pathZip) as DiskItemFileZip;
            IDiskItem? childItem = null;
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                childItem = DiskItemFabrick.GetDiskItem(zip, zipArchive.Entries[0]);
            }
            Assert.IsNotInstanceOfType(childItem, typeof(DiskItemError));
        }

        [TestMethod]
        public void GetDiskItem_pathZip_1_DiskItemFileFB2()
        {
            DiskItemFileZip? zip = DiskItemFabrick.GetDiskItem(pathZip) as DiskItemFileZip;
            IDiskItem? childItem = null;
            if(zip != null)
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                childItem = DiskItemFabrick.GetDiskItem(zip, zipArchive.Entries[0]);
            }
            Assert.IsInstanceOfType(childItem, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        public void GetDiskItem_pathAccess_IDiskItemError()
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathAccess);            
            Assert.IsInstanceOfType(item, typeof(DiskItemError));
        }      
    }
}