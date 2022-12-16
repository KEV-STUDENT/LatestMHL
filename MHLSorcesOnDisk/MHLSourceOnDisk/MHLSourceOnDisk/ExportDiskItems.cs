using MHLCommon;
using MHLCommon.MHLDiskItems;


namespace MHLSourceOnDisk
{
    public abstract class ExportDiskItems : IExport
    {
        #region [Fields]
        private readonly ExpOptions exportOptions;
        #endregion

        #region [Properties]
        protected ExpOptions ExportOptions => exportOptions;
        #endregion

        #region [Constructors]      
        public ExportDiskItems(ExpOptions expOptions)
        {
            exportOptions = expOptions;
        }
        #endregion

        #region [IExport Implementation]
        bool IExport.CheckDestination()
        {
            return CheckDestination();
        }
        bool IExport.Export(IDiskItem diskItem)
        {
            return Export(diskItem);
        }
        #endregion

        #region [Methods]
        public bool Export(IDiskItem diskItem)
        {
            return diskItem.ExportItem(exportOptions);
        }
        public abstract bool CheckDestination();
        #endregion
    }
}
