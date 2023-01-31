using MHLCommon.ExpDestinations;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class DiskItemFile : DiskItemExported, IFile
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

        public bool IsZipEntity => parent is DiskItemFileZip;
        #endregion

        #region [Protected Methods]
        protected virtual void Initialize()
        {
        }
        #endregion

        #region [DiskItem Implementation]
        public override bool ExportItem(IExportDestination destination)
        {
            bool result = true;
            if (destination is ExpDestination4Dir exp)
            {
                try
                {
                    File.Copy(this.Path2Item, exp.DestinationFullName, exp.OverWriteFiles);
                    result = File.Exists(exp.DestinationFullName);
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
        #endregion

        #region [IDiskCollection Implementation]
        IDiskCollection? IFile.Parent => parent;

        bool IFile.IsZipEntity => IsZipEntity;
        #endregion
    }
}