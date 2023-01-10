using MHLCommon.MHLBookDir;
using System.Text.Json;

namespace MHLCommon
{
    public static class MHLCommonStatic
    {
        public static int CompareStringByLength(string? x, string? y)
        {
            if (string.IsNullOrWhiteSpace(x) && string.IsNullOrWhiteSpace(y))
                return 0;

            if (string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y))
                return -1;

            if (!string.IsNullOrWhiteSpace(x) && string.IsNullOrWhiteSpace(y))
                return 1;

            if (x.Length < y.Length)
                return -1;

            if (y.Length < x.Length)
                return 1;

            return x.CompareTo(y);
        }

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

        public static T? GetRowFromJson<T>(string str)
        {
            T? row = JsonSerializer.Deserialize<T>(str);
            return row;
        }
    }
}
