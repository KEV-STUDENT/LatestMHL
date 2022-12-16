using MHLCommon;
using MHLCommon.MHLDiskItems;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Principal;

namespace MHLSourceOnDisk
{
    public class Export2Dir : ExportDiskItems
    {
        #region [Constructors]      
        public Export2Dir(ExpOptions expOptions) :base(expOptions) { }
        #endregion

        #region [Methods]
        public override bool CheckDestination()
        {
            if (!Directory.Exists(ExportOptions.PathDestination))
            {
               Directory.CreateDirectory(ExportOptions.PathDestination);
            }
            return Directory.Exists(ExportOptions.PathDestination);
        }
        #endregion
    }
}