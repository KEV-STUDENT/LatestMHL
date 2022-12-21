using MHLCommon.ExpDestinations;

namespace MHLCommon.MHLDiskItems
{
    public interface IDiskItem
    {
        IExport? Exporter { get; }
        string Path2Item { get; }
        string Name { get; }

        public bool ExportBooks<T>(T exporter) where T: class, IExport;
        public Task<bool> ExportBooksAsync<T>(T exporter) where T : class, IExport;
        public bool ExportItem(IExportDestination destination);
    }
}