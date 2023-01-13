using MHLCommon.ExpDestinations;

namespace MHLCommon.MHLDiskItems
{
    public interface IExport
    {
        ExpOptions ExportOptions { get; }
        public IExportDestination Destination { get; }
        public bool CheckDestination();
        bool Export(IDiskItemExported diskItem);
    }
}