using MHLSourceScannerModelLib;
using MHLSourceOnDisk;
using System.IO.Compression;
using MHLCommon.MHLDiskItems;

namespace MHLSourceScannerModelLibTest
{
    [TestClass]
    public class DiskItemVirtualGroupTest
    {
        protected static IDiskItem? GetDiskItemVirtualGroup (string path)
        {
            IDiskItem? item = null;

            if (DiskItemFabrick.GetDiskItem(path) is DiskItemFileZip zip)
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
        [DataRow(@"F:\1\test\fb2-495000-500999.zip")]
        public void GetDiskItemVirtualGroup_pathZip(string pathZip)
        {
            IDiskItem? item = GetDiskItemVirtualGroup(pathZip);
            if (item == null)
            {
                if (item is IVirtualGroup virtualGroup)
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
        [DataRow(@"F:\1\test\fb2-495000-500999.zip")]
        public void GetDiskItemFileFB2_pathZip(string pathZip)
        {
            IDiskItem? itemFB2 = null;
            if(GetDiskItemVirtualGroup(pathZip) is IVirtualGroup virtualGroup)
                itemFB2 = DiskItemFabrick.GetDiskItem(virtualGroup, virtualGroup.ItemsNames[0]);

            System.Diagnostics.Debug.WriteLine("----------");
            System.Diagnostics.Debug.WriteLine(itemFB2);
            System.Diagnostics.Debug.WriteLine("----------");

            Assert.IsInstanceOfType(itemFB2, typeof(DiskItemFileFB2));
        }
    }
}
