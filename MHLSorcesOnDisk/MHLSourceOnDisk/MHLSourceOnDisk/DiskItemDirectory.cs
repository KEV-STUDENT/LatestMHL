namespace MHLSourceOnDisk
{
    using MHLCommon;
    using MHLCommon.ExpDestinations;
    using MHLCommon.MHLBook;
    using MHLCommon.MHLDiskItems;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class DiskItemDirectory : DiskItem, IDiskCollection
    {

        #region [Constructors]
        public DiskItemDirectory(string path) : base(path, Path.GetFileName(path))
        {
        }
        #endregion

        #region [Properties]
        private string Path2Item
        {
            get => ((IDiskItem)this).Path2Item;
        }
        #endregion

        #region [Private Methods]
        private IEnumerable<IDiskItem> GetChilds()
        {

            IEnumerable<string>? dir = null;
            Exception? error = null;
            try
            {
                dir = GetChildsNames();
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (error != null)
            {
                yield return DiskItemFabrick.GetDiskItem(Path2Item, error);
            }
            else
            {
                if ((dir?.Count() ?? 0) > 0)
                {
                    ConcurrentBag<IDiskItem> res = new ConcurrentBag<IDiskItem>();
                    Parallel.ForEach(dir, item =>
                    {
                        IDiskItem diskItem = DiskItemFabrick.GetDiskItem(item);
                        res.Add(diskItem);
                    }
                    );
                    /*foreach (string item in dir)
                        yield return DiskItemFabrick.GetDiskItem(item);*/
                    foreach (IDiskItem item in res)
                        yield return item;
                }
            }
            yield break;
        }

        private IEnumerable<string> GetChildsNames()
        {
            foreach (string item in Directory.EnumerateDirectories(Path2Item))
                yield return item;

            foreach (string item in Directory.EnumerateFiles(Path2Item))
                yield return item;
        }
        #endregion


        #region [IDiskCollection implementation]
        int IDiskCollection.Count
        {
            get
            {
                return Directory.EnumerateDirectories(Path2Item).Count() + Directory.EnumerateFiles(Path2Item).Count();
            }
        }

        bool IDiskCollection.IsVirtualGroupsUsed => throw new NotImplementedException();

        IEnumerable<IDiskItem> IDiskCollection.GetChilds()
        {
            return GetChilds();
        }

        IEnumerable<IDiskItem> IDiskCollection.GetChilds(List<string> subList)
        {
            throw new NotImplementedException();
        }
        IEnumerable<string> IDiskCollection.GetChildsNames()
        {
            return GetChildsNames();
        }
        #endregion

        #region [DiskItem Implementation]
        public override bool ExportItem(IExportDestination destination)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}