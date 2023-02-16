using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using System.IO.Compression;

namespace MHLSourceOnDisk
{
    public static class MHLSourceOnDiskStatic
    {
        public static IMHLBook? GetBookFromZip(string pathZip, string fb2Name)
        {
            DiskItemFileZip zip = new DiskItemFileZip(pathZip);
            IMHLBook? itemFB2 = null;
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                ZipArchiveEntry? file = zipArchive.GetEntry(fb2Name);
                if (file != null)
                {
                    itemFB2 = DiskItemFabrick.GetDiskItem(zip, file) as IMHLBook;
                }
            }
            return itemFB2;
        }

        public static IDiskItem? GetItemFromZip(string pathZip, string fb2Name)
        {
            IDiskItem? itemFB2 = null;
            DiskItemFileZip zip = new DiskItemFileZip(pathZip);            
            using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
            {
                ZipArchiveEntry? file = zipArchive.GetEntry(fb2Name);
                if (file != null)
                {
                    itemFB2 = DiskItemFabrick.GetDiskItem(zip, file);
                }
            }
            return itemFB2;
        }
    }
}
