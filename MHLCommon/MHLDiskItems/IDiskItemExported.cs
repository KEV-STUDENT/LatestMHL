using MHLCommon.ExpDestinations;

namespace MHLCommon.MHLDiskItems
{
    public interface IDiskItemExported : IDiskItem
    {
        IExport? Exporter { get; }
        public bool ExportBooks<T>(T exporter) where T : class, IExport;
        public Task<bool> ExportBooksAsync<T>(T exporter) where T : class, IExport;
        public bool ExportItem(IExportDestination destination);
    }
}
