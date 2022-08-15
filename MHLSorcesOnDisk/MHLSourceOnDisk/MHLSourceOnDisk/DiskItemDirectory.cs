using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using MHLCommon;

namespace MHLSourceOnDisk
{
    using System.Collections.Generic;
    using System.ComponentModel;
    public class DiskItemDirectory : IDiskItemDirectory
    {
        private readonly string path;
        private readonly string name;

        public DiskItemDirectory(string path)
        {
            this.path = path;
            name = Path.GetFileName(path);
        }

        string IDiskItem.Path2Item => Path2Item;
        public string Path2Item => path;

        string IDiskItem.Name => Name;
        public string Name => name;

        int IDiskCollection.Count
        {
            get
            {
                return Directory.EnumerateDirectories(Path2Item).Count() + Directory.EnumerateFiles(Path2Item).Count();
            }
        }

        bool IDiskCollection.IsVirtualGroupsUsed => throw new NotImplementedException();

        private IEnumerable<IDiskItem> GetChilds()
        {

            IEnumerable<string>? dir = null;
            Exception? error = null;
            try
            {
                dir = Directory.EnumerateDirectories(path);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (error != null)
            {
                yield return DiskItemFabrick.GetDiskItem(path, error);
            }
            else
            {
                if (dir != null)
                {
                    foreach (string item in dir)
                        yield return DiskItemFabrick.GetDiskItem(item);
                }
            }

            dir = null;
            error = null;
            try
            {
                dir = Directory.EnumerateFiles(path);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (error != null)
            {
                yield return DiskItemFabrick.GetDiskItem(path, error);
            }
            else
            {
                if (dir != null)
                {
                    foreach (string item in dir)
                        yield return DiskItemFabrick.GetDiskItem(item);
                }
            }
            yield break;
        }

        IEnumerable<IDiskItem> IDiskCollection.GetChilds()
        {
            return GetChilds();
        }

        IEnumerable<IDiskItem> IDiskCollection.GetChilds(List<string> subList)
        {
            throw new NotImplementedException();
        }
    }
}