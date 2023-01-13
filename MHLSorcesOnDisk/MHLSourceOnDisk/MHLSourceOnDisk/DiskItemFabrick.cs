using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using System.IO.Compression;

namespace MHLSourceOnDisk
{
    internal enum FileType
    {
        Unknown = 0,
        Zip = 1,
        Fb2 = 2,
        Error = 3
    }
    public static class DiskItemFabrick
    {
        static readonly byte[] fileMarker = { 0xEF, 0xBB, 0xBF };
        static readonly byte[] fb2Signature = { 0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20 };
        static readonly byte[] zipSignature = { 0x50, 0x4B, 0x03, 0x04 };
        public static IDiskItem GetDiskItem(string path)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(path);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    if (attr.HasFlag(FileAttributes.System))
                    {
                        DiskItemDirectorySystem dirSys = new DiskItemDirectorySystem(path);
                        _ = ((IDiskCollection)dirSys).Count;
                        return dirSys;
                    }
                    else
                    {
                        DiskItemDirectory dir = new DiskItemDirectory(path);
                        _ = ((IDiskCollection)dir).Count;
                        return dir;
                    }
                }

                if (attr.HasFlag(FileAttributes.System))
                {
                    return new DiskItemFileSystem(path);
                }

                FileType type = FileType.Unknown;
                using (FileStream fileStream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    type = CheckFileType(fileStream);
                }

                switch (type)
                {
                    case FileType.Zip:
                        System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0}) - Zip", path);
                        return new DiskItemFileZip(path);
                    case FileType.Fb2:
                        System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0}) - FB2", path);
                        return new DiskItemFileFB2(path);
                    default:
                        System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0}) - File", path);
                        return new DiskItemFile(path);
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0}) - Error", path);
                System.Diagnostics.Debug.WriteLine(exp.Message);
                return new DiskItemError(path, exp);
            }
        }

        public static int CheckFileMarker(byte[] bytes)
        {
            byte[] fileHead = new byte[fileMarker.Length];

            if (bytes.Length >= fileMarker.Length)
            {
                Array.Copy(bytes, fileHead, fileHead.Length);

                if (Enumerable.SequenceEqual(fileMarker, fileHead))
                    return fileMarker.Length;
            }
            return 0;
        }

        public static IDiskItem GetDiskItem(IVirtualGroup virtualGroup, string itemName)
        {
            IDiskItem? diskItem = null;
            if (virtualGroup.ParentCollection is DiskItemFileZip zip)
            {
                using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
                {
                    ZipArchiveEntry? file = zipArchive.GetEntry(itemName);
                    if (file != null)
                        diskItem = DiskItemFabrick.GetDiskItem(zip, file);
                }
            }

            return diskItem ?? new DiskItemError(itemName, null);
        }

        public static IDiskItem GetDiskItem(IDiskCollection item, List<string> subList)
        {
            return new DiskItemVirtualGroup(item, subList);
        }

        public static IDiskItem GetDiskItem(DiskItemFileZip item, ZipArchiveEntry zipArchiveEntry)
        {
            System.Diagnostics.Debug.WriteLine("Thread : {0}  Task : {1}", Thread.CurrentThread.ManagedThreadId, Task.CurrentId);

            FileType type = FileType.Unknown;
            Exception? exp = null;
            DiskItemFileFB2? fb2 = null;
            bool nextTry;
            int ns;

            try
            {
                using (Stream fileStream = zipArchiveEntry.Open())
                {
                    type = CheckFileType(fileStream);

                    if (type == FileType.Fb2)
                    {
                        fb2 = new DiskItemFileFB2(item, zipArchiveEntry.FullName);
                        {
                            do
                            {                              
                                nextTry = string.IsNullOrEmpty(((IBook)fb2).Title);
                                if (nextTry)
                                {
                                    ns = fb2.CurrentNameSpace;
                                    fb2.CurrentNameSpace += 1;                                    
                                    if (ns == fb2.CurrentNameSpace)
                                    {
                                        nextTry = false;
                                        fb2 = null;
                                        type = FileType.Error;
                                    }
                                }

                            } while (nextTry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                fb2 = null;
                type = FileType.Error;
                exp = e;
            }

            switch (type)
            {
                case FileType.Zip:
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - Zip", item.Path2Item, zipArchiveEntry.FullName);
                    return new DiskItemFileZip(item, zipArchiveEntry.FullName);
                case FileType.Fb2:
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - FB2", item.Path2Item, zipArchiveEntry.FullName);
                    return fb2;
                case FileType.Error:
                    System.Diagnostics.Debug.WriteLine(exp?.Message ?? string.Empty);
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - Error", item.Path2Item, zipArchiveEntry.FullName);
                    return new DiskItemError(zipArchiveEntry.FullName, exp);
                default:
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - File", item.Path2Item, zipArchiveEntry.FullName);
                    return new DiskItemFile(item, zipArchiveEntry.FullName);
            }
        }

        internal static IDiskItem GetDiskItem(string path, Exception? error)
        {
            System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0}, {1}) - Error", path, error?.Message ?? "<Unknown error>");
            return new DiskItemError(path, error ?? new Exception("Unknown"));
        }

        private static FileType CheckFileType(Stream fileStream)
        {
            byte[] fileFB2Head = new byte[fb2Signature.Length + fileMarker.Length];
            byte[] fileFB2 = new byte[fb2Signature.Length];

            fileStream.Read(fileFB2Head, 0, fileFB2Head.Length);
            int markerLength = CheckFileMarker(fileFB2Head);

            if (markerLength > 0)
                Array.Copy(fileFB2Head, markerLength, fileFB2, 0, fb2Signature.Length);
            else
                Array.Copy(fileFB2Head, fileFB2, fb2Signature.Length);

            if (Enumerable.SequenceEqual(fileFB2, fb2Signature))
            {
                return FileType.Fb2;
            }

            byte[] fileZip = new byte[zipSignature.Length];
            Array.Copy(fileFB2, fileZip, zipSignature.Length);
            if (Enumerable.SequenceEqual(fileZip, zipSignature))
            {
                return FileType.Zip;
            }
            return FileType.Unknown;
        }

        public static async Task<IDiskItem> GetDiskItemAsync(DiskItemFileZip item, ZipArchiveEntry zipArchiveEntry)
        {
            FileType type = FileType.Unknown;
            Exception? exp = null;
            try
            {
                using (Stream fileStream = zipArchiveEntry.Open())
                {
                    type = await CheckFileTypeAsync(fileStream);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                type = FileType.Error;
                exp = e;
            }

            switch (type)
            {
                case FileType.Zip:
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - Zip", item.Path2Item, zipArchiveEntry.FullName);
                    return new DiskItemFileZip(item, zipArchiveEntry.FullName);
                case FileType.Error:
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - Error", item.Path2Item, zipArchiveEntry.FullName);
                    return new DiskItemError(zipArchiveEntry.FullName, exp);
                default:
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - File", item.Path2Item, zipArchiveEntry.FullName);
                    return new DiskItemFile(item, zipArchiveEntry.FullName);
            }
        }

        private static async Task<FileType> CheckFileTypeAsync(Stream fileStream)
        {
            byte[] fileFB2 = new byte[fb2Signature.Length];

            await fileStream.ReadAsync(fileFB2, 0, fb2Signature.Length);
            if (Enumerable.SequenceEqual(fileFB2, fb2Signature))
            {
                return FileType.Fb2;
            }

            byte[] fileZip = new byte[zipSignature.Length];
            Array.Copy(fileFB2, fileZip, zipSignature.Length);
            if (Enumerable.SequenceEqual(fileZip, zipSignature))
            {
                return FileType.Zip;
            }
            return FileType.Unknown;
        }

        public static bool ExportBooks<T>(IEnumerable<IDiskItemExported>? books, T exporter) where T : class, IExport
        {
            bool result = true;
            if ((books?.Count() ?? 0) > 0)
            {
                foreach (IDiskItemExported item in books)
                {
                    if (!item.ExportBooks<T>(exporter))
                        result = false;
                }
            }
            return result;
        }

        public static bool ExportBooks<T>(IDiskItemExported? book, T exporter) where T : class, IExport
        {
            return book?.ExportBooks<T>(exporter) ?? false;
        }

    }
}