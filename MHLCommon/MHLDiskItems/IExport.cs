using MHLCommon.ExpDestinations;

namespace MHLCommon.MHLDiskItems
{
    public interface IExport
    {
        ExpOptions ExportOptions { get; }
        public IExportDestination Destination { get; }

        bool CheckCreateDir(string dir);
        public bool CheckDestination();
        bool Export(IDiskItemExported diskItem);
    }
}