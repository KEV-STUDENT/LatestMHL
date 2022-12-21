using MHLCommon.ExpDestinations;
using MHLCommon.MHLBook;

namespace MHLCommon.MHLDiskItems
{
    public abstract class DiskItem : IDiskItem
    {
        #region [Fields]
        private readonly string path2Item;
        private readonly string name;
        private IExport? exporter = null;
        #endregion

        #region [Constructors]
        public DiskItem(string path, string name)
        {
            path2Item = path;
            this.name = name;
        }
        #endregion

        #region [Methods]
        public abstract bool ExportItem(IExportDestination destination);

        public bool ExportBooks<T>(T exporter) where T : class, IExport
        {
            this.exporter = exporter;

            bool result;
            System.Diagnostics.Debug.WriteLine("Export Books");

            result = exporter.CheckDestination();

            if (result)
            {
                result = exporter.Export(this);
            }

            return result;
        }

        public async Task<bool> ExportBooksAsync<T>(T exporter) where T : class, IExport
        {
           return  await Task<bool>.Run(()=> { return ExportBooks(exporter); });
        }
        #endregion

        #region [Properties]
        public IExport? Exporter { get => exporter; }
        #endregion

        #region [IDiskItem implementation]
        string IDiskItem.Path2Item => path2Item;


        string IDiskItem.Name => name;

        bool IDiskItem.ExportBooks<T>(T exporter)
        {
           return ExportBooks(exporter);
        }
        bool IDiskItem.ExportItem(IExportDestination destination)
        {
            return ExportItem(destination);
        }

        IExport? IDiskItem.Exporter { get => Exporter; }
        #endregion
    }
}
