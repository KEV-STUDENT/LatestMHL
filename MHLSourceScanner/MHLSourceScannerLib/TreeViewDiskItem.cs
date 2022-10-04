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
        public TreeViewUnknown(string path, ITreeItem? parent) : base(path, parent)
        {
        }

        public TreeViewUnknown(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewUnknown(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }

        public TreeViewUnknown(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
    }
    public class TreeViewSystem : TreeViewDiskItem<Decor4System>
    {
        public TreeViewSystem(string path, ITreeItem? parent) : base(path, parent)
        {
        }
        public TreeViewSystem(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewSystem(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }
        public TreeViewSystem(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
    }

    public class TreeViewError : TreeViewDiskItem<Decor4Error>
    {
        public TreeViewError(ITreeItem? parent) : base(parent)
        {
        }
        public TreeViewError(string path, ITreeItem? parent) : base(path,parent)
        {
        }

        public TreeViewError(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewError(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }

        public TreeViewError(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
    }

    public abstract class TreeViewDiskItem<T> : TreeDiskItem where T : IDecorator, new()
    {
        private readonly T decorator = new T();

        public TreeViewDiskItem(ITreeItem? parent) :base(parent)
        {
        }
        public TreeViewDiskItem(IShower? shower, ITreeItem? parent) : base(shower, parent)
        {
        }

        public TreeViewDiskItem(string path, ITreeItem? parent) : base(path, parent)
        {
        }
        public TreeViewDiskItem(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewDiskItem(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }
        public TreeViewDiskItem(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
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
                newTreeItem = new TreeViewFB2(diskItemChild, shower, this);
            else if (diskItemChild is DiskItemFileZip)
                newTreeItem = new TreeViewZip(diskItemChild, shower, this);
            else if (diskItemChild is IDiskItemVirtualGroup)
                newTreeItem = new TreeViewVirtualGroup(diskItemChild, shower, this);
            else if (diskItemChild is DiskItemDirectorySystem || diskItemChild is DiskItemFileSystem)
                newTreeItem = new TreeViewSystem(diskItemChild, shower, this);
            else if (diskItemChild is IDiskItemDirectory)
                newTreeItem = new TreeViewDirectory(diskItemChild, shower, this);
            else if (diskItemChild is IDiskItemError)
                newTreeItem = new TreeViewError(diskItemChild, shower, this);
            else newTreeItem = new TreeViewUnknown(diskItemChild, shower, this);

            return newTreeItem;
        }
    }
}
