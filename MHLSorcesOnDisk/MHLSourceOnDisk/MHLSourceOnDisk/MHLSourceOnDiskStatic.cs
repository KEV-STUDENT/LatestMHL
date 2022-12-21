using MHLCommon.MHLBook;
using System.IO.Compression;

namespace MHLSourceOnDisk
{
    public static class MHLSourceOnDiskStatic
    {
        public static IBook? GetBookFromZip(string pathZip, string fb2Name)
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
