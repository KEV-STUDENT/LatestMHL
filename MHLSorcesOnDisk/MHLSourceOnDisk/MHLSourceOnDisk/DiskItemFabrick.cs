using System.IO.Compression;
using MHLCommon.MHLDiskItems;

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
                        DiskItemDirectorySystem dirSys =  new DiskItemDirectorySystem(path);
                        _ = ((IDiskCollection)dirSys).Count;
                        return dirSys;
                    }
                    else
                    {
                        DiskItemDirectory dir =  new DiskItemDirectory(path);
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

                switch(type)
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

        public static IDiskItem GetDiskItem(IDiskItemVirtualGroup virtualGroup, string itemName)
        {
            IDiskItem diskItem = null;
            if(virtualGroup.ParentCollection is DiskItemFileZip zip)
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
            try
            {
                using (Stream fileStream = zipArchiveEntry.Open())
                {
                    type = CheckFileType(fileStream);
                }
            }catch(Exception e)
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
                case FileType.Fb2:
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - FB2", item.Path2Item, zipArchiveEntry.FullName);
                    return new DiskItemFileFB2(item, zipArchiveEntry.FullName);
                case FileType.Error:
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - Error", item.Path2Item, zipArchiveEntry.FullName);
                    return new DiskItemError(zipArchiveEntry.FullName, exp);
                default:
                    System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0},{1}) - File", item.Path2Item, zipArchiveEntry.FullName);
                    return new DiskItemFile(item, zipArchiveEntry.FullName);
            }
        }

        internal static IDiskItem GetDiskItem(string path, Exception? error)
        {
            System.Diagnostics.Debug.WriteLine("DiskItemFabrick.GetDiskItem({0}, {1}) - Error", path, error?.Message??"<Unknown error>");
            return new DiskItemError(path, error ?? new Exception("Unknown"));
        }

        private static FileType CheckFileType (Stream fileStream)
        {
            byte[] fileFB2 = new byte[fb2Signature.Length];
           
            fileStream.Read(fileFB2, 0, fb2Signature.Length);
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
    }
}