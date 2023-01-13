using MHLCommon.ExpDestinations;

namespace MHLCommon.MHLDiskItems
{
    public abstract class DiskItemExported : DiskItem, IDiskItemExported
    {
        #region [Fields]
        private IExport? exporter = null;
        #endregion

        #region [Constructors]
        public DiskItemExported(string path, string name) : base(path, name) { }
        #endregion

        #region [Methods]
        public abstract bool ExportItem(IExportDestination destination);

        public bool ExportBooks<T>(T exporter) where T : class, IExport
        {
            this.exporter = exporter;
            bool result;

            result = exporter.CheckDestination();

            if (result)
            {
                result = exporter.Export(this);
            }

            return result;
        }

        public async Task<bool> ExportBooksAsync<T>(T exporter) where T : class, IExport
        {
            return await Task<bool>.Run(() => { return ExportBooks(exporter); });
        }
        #endregion

        #region [Properties]
        public IExport? Exporter { get => exporter; }
        #endregion

        #region [IDiskItemExported implementation]
        bool IDiskItemExported.ExportBooks<T>(T exporter)
        {
            return ExportBooks(exporter);
        }
        bool IDiskItemExported.ExportItem(IExportDestination destination)
        {
            return ExportItem(destination);
        }

        IExport? IDiskItemExported.Exporter { get => Exporter; }
        #endregion
    }
}
