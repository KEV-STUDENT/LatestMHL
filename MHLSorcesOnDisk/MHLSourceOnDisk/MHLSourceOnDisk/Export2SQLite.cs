using MHLCommon;
using MHLCommon.ExpDestinations;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class Export2SQLite : ExportDiskItems
    {
        #region [Constructors]
        public Export2SQLite(ExpOptions expOptions) : base(expOptions) { }       
        #endregion

        #region [Methods]
        public override bool CheckDestination()
        {
            if(Destination is ExpDestination2SQLite expDestination)
                return CheckCreateDir(Path.GetDirectoryName(expDestination.DestinationPath)??string.Empty);

            return false;
        }

        protected override IExportDestination CreateDestinantion(IDiskItem? diskItem)
        {
            return new ExpDestination2SQLite(ExportOptions.PathDestination);
        }
        #endregion


    }
}