namespace MHLCommon.MHLDiskItems
{
    public abstract class DiskItem : IDiskItem
    {
        #region [Fields]
        private readonly string path2Item;
        private readonly string name;
        #endregion

        #region [Constructors]
        public DiskItem(string path, string name)
        {
            path2Item = path;
            this.name = name;
        }
        #endregion

        #region [Methods]
        public abstract bool ExportItem(ExpOptions exportOptions);

        public bool ExportBooks<T>(T exporter) where T : class, IExport
        {
            bool result = false;
            System.Diagnostics.Debug.WriteLine("Export Books");  
            if (exporter.CheckDestination())
            {
                result = exporter.Export(this);
            }

            return result;
        }
        #endregion

        #region [IDiskItem implementation]
        string IDiskItem.Path2Item => path2Item;


        string IDiskItem.Name => name;

        bool IDiskItem.ExportBooks<T>(T exporter)
        {
           return ExportBooks(exporter);
        }
        bool IDiskItem.ExportItem(ExpOptions exportOptions)
        {
            return ExportItem(exportOptions);
        }
        #endregion
    }
}
