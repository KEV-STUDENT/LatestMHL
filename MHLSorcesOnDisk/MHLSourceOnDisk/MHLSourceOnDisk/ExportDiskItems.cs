using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.ExpDestinations;

namespace MHLSourceOnDisk
{
    public abstract class ExportDiskItems : IExport
    {
        #region [Fields]
        private readonly ExpOptions exportOptions;
        protected IExportDestination destination;
        #endregion

        #region [Properties]
        protected ExpOptions ExportOptions => exportOptions;
        public IExportDestination Destination=> destination;        
        #endregion

        #region [Constructors]      
        public ExportDiskItems(ExpOptions expOptions) : this(expOptions, null) { }
        public ExportDiskItems(ExpOptions expOptions, IDiskItem? diskItem)
        {
            exportOptions = expOptions;
            destination = CreateDestinantion(diskItem);
        }
        #endregion

        #region [IExport Implementation]
        ExpOptions IExport.ExportOptions => ExportOptions;
        IExportDestination IExport.Destination => Destination;

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
            return diskItem.ExportItem(Destination);
        }
        public abstract bool CheckDestination();       
        protected abstract IExportDestination CreateDestinantion(IDiskItem? diskItem);
        #endregion
    }
}
