using MHLCommon.MHLDiskItems;
using System.IO.Compression;

namespace MHLSourceOnDisk
{
    public class Export2Dir : IExport
    {
        #region [Fields]
        private string pathDestination;
        #endregion

        #region [Constructors]
        public Export2Dir(string pathDestination)
        {
            this.pathDestination = pathDestination;
        }
        #endregion

        #region [Methods]
        #endregion

        #region [IExport implementation]
        bool IExport.CheckDestination()
        {
            if(!Directory.Exists(pathDestination))
                Directory.CreateDirectory(pathDestination);

            return Directory.Exists(pathDestination);
        }

        bool IExport.Export(IDiskItem diskItem)
        {
            bool result = false;
            try
            {
                if (diskItem is IZip)
                {
                    ZipFile.ExtractToDirectory(diskItem.Path2Item, pathDestination, true);
                    result = true;
                }
            }catch (Exception)
            {
                result = false;
            }
            return result;
        }
        #endregion
    }
}