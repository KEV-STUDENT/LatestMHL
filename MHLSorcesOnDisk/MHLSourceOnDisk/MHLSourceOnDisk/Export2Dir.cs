using MHLCommon;
using MHLCommon.MHLDiskItems;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Principal;

namespace MHLSourceOnDisk
{
    public class Export2Dir : IExport
    {
        #region [Fields]
        private ExpOptions exportOptions;
        #endregion

        #region [Constructors]      
        public Export2Dir(ExpOptions expOptions)
        {
            exportOptions = expOptions;
        }
        #endregion

        #region [Methods]
        #endregion

        #region [IExport implementation]
        bool IExport.CheckDestination()
        {
            if (!Directory.Exists(exportOptions.PathDestination))
            {
               Directory.CreateDirectory(exportOptions.PathDestination);
            }
            return Directory.Exists(exportOptions.PathDestination);
        }

        bool IExport.Export(IDiskItem diskItem)
        {
            return diskItem.ExportItem(exportOptions);
        }
        #endregion
    }
}