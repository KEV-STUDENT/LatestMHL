using MHLCommon.MHLBook;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLSourceOnDisk
{
    public static class MHLSourceOnDiskStatic
    {
        public static string GetNewFileName(string pathDestination, string file)
        {
            string newFile, name, ext;
            int i = 0;

            name = Path.GetFileNameWithoutExtension(file);
            ext = Path.GetExtension(file);

            newFile = Path.Combine(pathDestination, file);
            while (File.Exists(newFile))
            {
                newFile = Path.Combine(pathDestination, Path.ChangeExtension(string.Format("{0}({1})", name, ++i), ext));
            }
            return newFile;
        }

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
