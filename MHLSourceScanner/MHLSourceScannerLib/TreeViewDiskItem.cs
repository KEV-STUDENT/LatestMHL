using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceOnDisk;
using MHLSourceScannerModelLib;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public class TreeViewUnknown : TreeViewDiskItem<Decor4Unknown>
    {
        public TreeViewUnknown(string path) : base(path)
        {
        }

        public TreeViewUnknown(string path, IShower? shower) : base(path, shower)
        {
        }

        public TreeViewUnknown(IDiskItem diskItemSource) : base(diskItemSource)
        {
        }

        public TreeViewUnknown(IDiskItem diskItemSource, IShower? shower) : base(diskItemSource, shower)
        {
        }
    }
    public class TreeViewSystem : TreeViewDiskItem<Decor4System>
    {
        public TreeViewSystem(string path) : base(path)
        {
        }
        public TreeViewSystem(string path, IShower? shower) : base(path, shower)
        {
        }

        public TreeViewSystem(IDiskItem diskItemSource) : base(diskItemSource)
        {
        }
        public TreeViewSystem(IDiskItem diskItemSource, IShower? shower) : base(diskItemSource, shower)
        {
        }
    }

    public class TreeViewError : TreeViewDiskItem<Decor4Error>
    {
        public TreeViewError() : base()
        {
        }
        public TreeViewError(string path) : base(path)
        {
        }

        public TreeViewError(string path, IShower? shower) : base(path, shower)
        {
        }

        public TreeViewError(IDiskItem diskItemSource) : base(diskItemSource)
        {
        }

        public TreeViewError(IDiskItem diskItemSource, IShower? shower) : base(diskItemSource, shower)
        {
        }
    }

    public class TreeViewDiskItem<T> : TreeDiskItem where T : IDecorator, new()
    {
        private readonly T decorator = new T();

        public TreeViewDiskItem():base()
        {
        }
        public TreeViewDiskItem(IShower? shower) : base(shower)
        {
        }

        public TreeViewDiskItem(string path) : base(path)
        {
        }
        public TreeViewDiskItem(string path, IShower? shower) : base(path, shower)
        {
        }

        public TreeViewDiskItem(IDiskItem diskItemSource) : base(diskItemSource)
        {
        }
        public TreeViewDiskItem(IDiskItem diskItemSource, IShower? shower) : base(diskItemSource, shower)
        {
        }

        public Brush ForeGround
        {
            get => decorator.ForeGround;
        }

        public FontWeight FontWeight
        {
            get => decorator.FontWeight;
        }

        public bool Focusable
        {
            get => decorator.Focusable;
        }


        public override ITreeDiskItem CreateTreeViewItem(IDiskItem diskItemChild)
        {
            ITreeDiskItem newTreeItem;

            if (diskItemChild is IBook)
                newTreeItem = new TreeViewFB2(diskItemChild, shower);
            else if (diskItemChild is DiskItemFileZip)
                newTreeItem = new TreeViewZip(diskItemChild, shower);
            else if (diskItemChild is IDiskItemVirtualGroup)
                newTreeItem = new TreeViewVirtualGroup(diskItemChild, shower);
            else if (diskItemChild is DiskItemDirectorySystem || diskItemChild is DiskItemFileSystem)
                newTreeItem = new TreeViewSystem(diskItemChild, shower);
            else if (diskItemChild is IDiskItemDirectory)
                newTreeItem = new TreeViewDirectory(diskItemChild, shower);
            else if (diskItemChild is IDiskItemError)
                newTreeItem = new TreeViewError(diskItemChild, shower);
            else newTreeItem = new TreeViewUnknown(diskItemChild, shower);

            return newTreeItem;
        }
    }
}
