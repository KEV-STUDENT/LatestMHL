using System.IO;
using MHLCommon;
using MHLCommon.ExpDestinations;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class DiskItemFile : DiskItem, IFile
    {
        #region [Private Fields]
        private readonly IDiskCollection? parent;
        #endregion

        #region [Constructors]
        public DiskItemFile(string path) : base(path, Path.GetFileName(path))
        {
            Initialize();
        }

        public DiskItemFile(DiskItemFileZip zip, string fullName) : base(zip.Path2Item, fullName)
        {
            parent = zip;
            Initialize();
        }
        #endregion

        #region [Public Properties]
        public string Path2Item => ((IDiskItem)this).Path2Item;
        public string Name => ((IDiskItem)this).Name;
        #endregion

        #region [Protected Methods]
        protected virtual void Initialize()
        {
        }
        #endregion

        #region [DiskItem Implementation]
        public override bool ExportItem(IExportDestination destination)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region [IDiskCollection Implementation]
        IDiskCollection? IFile.Parent => parent;
        #endregion
    }
}