using System.IO;
using MHLCommon;

namespace MHLSourceOnDisk
{
    public class DiskItemFile : IDiskItemFile
    {
        private readonly string path;
        private readonly string name;

        private readonly IDiskCollection? parent;

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

        string IDiskItem.Path2Item => Path2Item;
        public string Path2Item => path;

        string IDiskItem.Name => Name;
        public string Name { get => name;}

        IDiskCollection? IDiskItemFile.Parent => parent;

        protected virtual void Initialize()
        {
        }
    }
}