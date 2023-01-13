using MHLCommon.ExpDestinations;

namespace MHLCommon.MHLDiskItems
{
    public abstract class DiskItem : IDiskItem
    {
        #region [Fields]
        private readonly string path2Item;
        private readonly string name;
        private IExport? exporter = null;
        #endregion

        #region [Constructors]
        public DiskItem(string path, string name)
        {
            path2Item = path;
            this.name = name;
        }
        #endregion

        #region [Properties]
        public IExport? Exporter { get => exporter; }
        #endregion

        #region [IDiskItem implementation]
        string IDiskItem.Path2Item => path2Item;
        string IDiskItem.Name => name;
        #endregion
    }
}
