using MHLSourceScannerModelLib;
using MHLSourceOnDisk;
using System.IO.Compression;
using MHLCommon.MHLDiskItems;

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

            if (zip != null)
            {
                using ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item);
                {
                    List<string> subList = (
                            from file in zipArchive.Entries
                            select file.Name).ToList();


                    item = DiskItemFabrick.GetDiskItem(zip, subList);
                }
            }

            return item;
        }

        [TestMethod]
        public void GetDiskItemVirtualGroup_pathZip()
        {
            IDiskItem? item = GetDiskItemVirtualGroup(pathZip);
            if (item == null)
            {
                IVirtualGroup? virtualGroup = item as IVirtualGroup;
                if (virtualGroup != null)
                {
                    foreach (string file in virtualGroup.ItemsNames)
                    {
                        System.Diagnostics.Debug.WriteLine(file);
                    }
                }
            }
            Assert.IsInstanceOfType(item, typeof(DiskItemVirtualGroup));
        }

        [TestMethod]
        public void GetDiskItemFileFB2_pathZip()
        {
            IDiskItem? item = GetDiskItemVirtualGroup(pathZip);
            IVirtualGroup? virtualGroup = item as IVirtualGroup;

            IDiskItem? itemFB2 = null;
            if(virtualGroup != null)
                itemFB2 = DiskItemFabrick.GetDiskItem(virtualGroup, virtualGroup.ItemsNames[0]);

            System.Diagnostics.Debug.WriteLine("----------");
            System.Diagnostics.Debug.WriteLine(itemFB2);
            System.Diagnostics.Debug.WriteLine("----------");

            Assert.IsInstanceOfType(itemFB2, typeof(DiskItemFileFB2));
        }
    }
}
