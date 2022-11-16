using MHLCommon.MHLBook;
using System.IO.Compression;

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
    }
}
