using MHLCommon;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO.Compression;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class DiskItemFabrickTest
    {
        [TestMethod]
        [DataRow(@"F:\1\test")]
        public void GetDiskItem_pathDir_IDiskItem(string pathDir)
        {            
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);

            Assert.IsNotNull(item);
        }

        [TestMethod]
        [DataRow(@"F:\1\test")]
        public void GetDiskItem_IDiskItemDirectory(string pathDir)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathDir);
            Assert.IsInstanceOfType(item, typeof(IDiskCollection));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void GetDiskItem_IDiskItemFile(string pathFile)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathFile);
            Assert.IsInstanceOfType(item, typeof(IFile));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void GetDiskItem_DiskItemFileFB2(string pathFile)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathFile);
            Assert.IsInstanceOfType(item, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip")]
        public void GetDiskItem_DiskItemFileZip(string pathZip)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathZip);
            Assert.IsInstanceOfType(item, typeof(DiskItemFileZip));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip")]
        [DataRow(@"F:\1\test")]
        public void GetDiskItem_IDiskCollection(string pathZip)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathZip);
            Assert.IsInstanceOfType(item, typeof(IDiskCollection));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", 0)]
        public void GetDiskItem_NotDiskItemError(string pathZip, int entryIndex)
        {
            DiskItemFileZip? zip = DiskItemFabrick.GetDiskItem(pathZip) as DiskItemFileZip;
            IDiskItem? childItem = null;
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                childItem = DiskItemFabrick.GetDiskItem(zip, zipArchive.Entries[entryIndex]);
            }
            Assert.IsNotInstanceOfType(childItem, typeof(DiskItemError));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", 0)]
        public void GetDiskItem_DiskItemFileFB2_ZIP(string pathZip, int entryIndex)
        {
            IDiskItem? childItem = null;
            if(DiskItemFabrick.GetDiskItem(pathZip) is DiskItemFileZip zip)
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                childItem = DiskItemFabrick.GetDiskItem(zip, zipArchive.Entries[entryIndex]);
            }
            Assert.IsInstanceOfType(childItem, typeof(DiskItemFileFB2));
        }

        [TestMethod]
        [DataRow(@"C:\Config.Msi")]
        [DataRow(@"F1:\1\test\426096.fb2")]
        public void GetDiskItem_DiskItemError(string pathAccess)
        {
            IDiskItem item = DiskItemFabrick.GetDiskItem(pathAccess);
            Assert.IsInstanceOfType(item, typeof(DiskItemError));
        }
        
        [TestMethod]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-060424-074391.zip", "60481.fb2")]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "498446.fb2")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426649.fb2")]
        public void GetDiskItem_DiskItemFB2_name_ZIP(string path2Zip, string nameFB2)
        {

            DiskItemFileZip zip = new DiskItemFileZip(path2Zip);
            IDiskItem? item = null;
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                ZipArchiveEntry? file = zipArchive.GetEntry(nameFB2);
                if (file != null)
                    item = DiskItemFabrick.GetDiskItem(zip, file);
            }
            Assert.IsInstanceOfType(item, typeof(DiskItemFileFB2));
        }


        [TestMethod]
        [DataRow(@"F:\1\test\destination", @"F:\1\test\fb2-495000-500999.zip", true)]
        [DataRow(@"F:\1\test\destination", @"F:\1\test\fb2-495000-500999.zip", false)]
        [DataRow(@"F:\1\test\destination", @"F:\1\test\426096.fb2", true)]
        [DataRow(@"F:\1\test\destination", @"F:\1\test\426096.fb2", false)]
        public void ExportBooks(string pathDestination, string pathFile, bool overWriteFile)
        {
            bool res = false;
            ExpOptions expOptions = new ExpOptions(pathDestination, overWriteFile);
            Export2Dir exporter;

            IDiskItem item = DiskItemFabrick.GetDiskItem(pathFile);
            if (item is IDiskCollection diskCollection)
            {
                exporter = new Export2Dir(expOptions);
                IEnumerable<IDiskItemExported>? books = (diskCollection.GetChilds() as IEnumerable<IDiskItemExported>);
                res = DiskItemFabrick.ExportBooks(books, exporter);
            }
            else
            {
                exporter = new Export2Dir(expOptions, item);
                if (item is IDiskItemExported exported)
                    res = DiskItemFabrick.ExportBooks(exported, exporter);
            }
            Assert.IsTrue(res);
        }       
    }
}