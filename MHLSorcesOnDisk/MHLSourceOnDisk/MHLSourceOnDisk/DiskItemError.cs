using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class DiskItemError : IDiskItemError
    {
        private readonly string path;
        private readonly string name;
        private readonly Exception? exp;

        public DiskItemError(string path, Exception? exp)
        {
            this.path = path;
            this.exp = exp;
            name = Path.GetFileName(path);
        }

        string IDiskItem.Path2Item => Path2Item;
        public string Path2Item => path;


        string IDiskItem.Name => Name;
        public string Name =>name;
    }
}