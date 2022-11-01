namespace MHLCommon.MHLDiskItems
{
    public interface IDiskItem
    {
        string Path2Item { get; }
        string Name { get; }

        public bool ExportBooks<T>(T exporter) where T: class, IExport;
        public bool ExportItem(ExpOptions exportOptions);
    }
}