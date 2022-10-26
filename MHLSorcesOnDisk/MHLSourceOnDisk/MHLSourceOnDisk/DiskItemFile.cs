using System.IO;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class DiskItemFile : IDiskItemFile
    {
        #region [Private Fields]
        private readonly string path;
        private readonly string name;

        private readonly IDiskCollection? parent;
        #endregion

        #region [Constructors]
        public DiskItemFile(string path)
        {
            this.path = path;
            name = Path.GetFileName(path);
            Initialize();
        }

        public DiskItemFile(DiskItemFileZip zip, string fullName)
        {
            path = zip.Path2Item;
            name = fullName;
            parent = zip;

            Initialize();
        }
        #endregion

        #region [Public Properties]
        public string Path2Item => path;
        public string Name { get => name;}
        #endregion

        #region [Protected Methods]
        protected virtual void Initialize()
        {
        }
        #endregion

        #region [IDiskItem Implementation]
        string IDiskItem.Path2Item => Path2Item;
        string IDiskItem.Name => Name;
        bool IDiskItem.ExportBooks<T>(T exporter)
        {
            bool result = false;

            if (exporter.CheckDestination())
            {
                result = exporter.Export((IDiskItem)this);
            }

            return result;
        }
        #endregion

        #region [IDiskCollection Implementation]
        IDiskCollection? IDiskItemFile.Parent => parent;
        #endregion
    }
}