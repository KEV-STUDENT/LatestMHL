using MHLCommon;
using MHLCommon.ExpDestinations;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class DiskItemError : DiskItem
    {
        #region [Private Fields]
        private readonly Exception? exp;
        #endregion

        #region [Constructor]
        public DiskItemError(string path, Exception? exp) : base(path, Path.GetFileName(path))
        {
            this.exp = exp;
        }
        #endregion

        #region [Public Properties]
        public string Name => ((IDiskItem)this).Name;
        public string Path2Item => ((IDiskItem)this).Path2Item;
        #endregion

        #region [DiskItem Implentation]
        public override bool ExportItem(IExportDestination destination)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}