using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLSourceOnDisk
{
    internal static class MHLSourceOnDiskStatic
    {
        internal static string GetNewFileName(string pathDestination, string file)
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
    }
}
