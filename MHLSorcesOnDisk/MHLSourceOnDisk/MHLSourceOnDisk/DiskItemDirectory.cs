namespace MHLSourceOnDisk
{
    using MHLCommon;
    using MHLCommon.MHLDiskItems;
    using System.Collections.Generic;

    public class DiskItemDirectory : DiskItem, IDiskItemDirectory
    {

        #region [Constructors]
        public DiskItemDirectory(string path):base(path, Path.GetFileName(path))
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
                dir = Directory.EnumerateDirectories(Path2Item);
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
                dir = Directory.EnumerateFiles(Path2Item);
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
                if (dir != null)
                {
                    foreach (string item in dir)
                        yield return DiskItemFabrick.GetDiskItem(item);
                }
            }
            yield break;
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
        #endregion

        #region [DiskItem Implementation]
        public override bool ExportItem(ExpOptions exportOptions)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}