using MHLSourceScannerModelLib;
using MHLSourceOnDisk;
using MHLCommon;
using System.IO.Compression;


namespace MHLSourceScannerModelLibTest
{
    [TestClass]
    public class DiskItemVirtualGroupTest
    {
        protected string pathZip = @"F:\1\test\fb2-495000-500999.zip";

        protected IDiskItem? GetDiskItemVirtualGroup (string path)
        {
            DiskItemFileZip? zip = DiskItemFabrick.GetDiskItem(path) as DiskItemFileZip;
            IDiskItem? item = null;

            using ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item);
            {
                List<string> subList = (
                        from file in zipArchive.Entries
                        select file.Name).ToList();


                item = DiskItemFabrick.GetDiskItem(zip, subList);
            }

            return item;
        }

        [TestMethod]
        public void GetDiskItemVirtualGroup_pathZip()
        {
            IDiskItem? item = GetDiskItemVirtualGroup(pathZip);
            System.Diagnostics.Debug.WriteLine(item.Path2Item);

            IDiskItemVirtualGroup? virtualGroup = item as IDiskItemVirtualGroup;
            foreach(string file in virtualGroup.ItemsNames)
            {
                System.Diagnostics.Debug.WriteLine(file);
            }

            Assert.IsInstanceOfType(item, typeof(DiskItemVirtualGroup));
        }

        [TestMethod]
        public void GetDiskItemFileFB2_pathZip()
        {
            IDiskItem? item = GetDiskItemVirtualGroup(pathZip);
            IDiskItemVirtualGroup? virtualGroup = item as IDiskItemVirtualGroup;

            IDiskItem? itemFB2 = DiskItemFabrick.GetDiskItem(virtualGroup, virtualGroup.ItemsNames[0]);
           

            Assert.IsInstanceOfType(itemFB2, typeof(DiskItemFileFB2));
        }
    }
}
