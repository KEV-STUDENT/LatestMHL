using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class DiskItemError : IDiskItemError
    {
        #region [Private Fields]
        private readonly string path;
        private readonly string name;
        private readonly Exception? exp;
        #endregion

        #region [Constructor]
        public DiskItemError(string path, Exception? exp)
        {
            this.path = path;
            this.exp = exp;
            name = Path.GetFileName(path);
        }
        #endregion

        #region [Public Properties]
        public string Name => name;
        public string Path2Item => path;
        #endregion

        #region [IDiskItem Implentation]
        string IDiskItem.Path2Item => Path2Item;

        string IDiskItem.Name => Name;

        bool IDiskItem.ExportBooks<T>(T exporter)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}