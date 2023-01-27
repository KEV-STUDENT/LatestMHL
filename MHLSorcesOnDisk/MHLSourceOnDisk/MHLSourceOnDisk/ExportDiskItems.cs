using MHLCommon;
using MHLCommon.ExpDestinations;
using MHLCommon.MHLDiskItems;

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
        bool IExport.Export(IDiskItemExported diskItem)
        {
            return Export(diskItem);
        }
        bool IExport.CheckCreateDir(string dir)
        {
            return CheckCreateDir(dir);
        }
        #endregion

        #region [Methods]
        public bool Export(IDiskItemExported diskItem)
        {
            return diskItem.ExportItem(Destination);
        }
        protected virtual bool CheckCreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return Directory.Exists(dir);
        }
        public abstract bool CheckDestination();       
        protected abstract IExportDestination CreateDestinantion(IDiskItem? diskItem);
        #endregion
    }
}
