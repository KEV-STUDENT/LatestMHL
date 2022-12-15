using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceOnDisk;

namespace MHLSourceScannerModelLib
{
    public class TreeDiskItem : TreeItemCollection, ITreeDiskItem
    {
        protected IShower? shower;

        private readonly object sourceLock = new object();

        #region [Properties]
        public string Path2Item => Source?.Path2Item ?? String.Empty;
        string ITreeDiskItem.Path2Item => Path2Item;
        public IShower? Shower { get => shower; set => shower = value; }

        public bool TestMode { get; set; }
        #endregion

        #region [Constructors]
        public TreeDiskItem(ITreeItem? parent) : base(parent)
        {
            this.shower = null;
        }
        public TreeDiskItem(IShower? shower, ITreeItem? parent) : this(parent)
        {
            this.shower = shower;
        }

        public TreeDiskItem(string path, ITreeItem? parent) : this(path, null, parent)
        {
        }
        public TreeDiskItem(string path, IShower? shower, ITreeItem? parent) : this(shower, parent)
        {
            Source = DiskItemFabrick.GetDiskItem(path);
        }

        public TreeDiskItem(IDiskItem diskItemSource, ITreeItem? parent) : this(diskItemSource, null, parent)
        {
            Source = diskItemSource;
        }
        public TreeDiskItem(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : this(shower, parent)
        {
            Source = diskItemSource;
        }
        #endregion


        #region [ITreeDiskItem implementation]
        void ITreeDiskItem.AddDiskItem(IDiskItem diskItemChild)
        {
            AddDiskItem(diskItemChild);
        }

        ITreeItem ITreeDiskItem.CreateTreeViewItem(IDiskItem diskItemChild)
        {
            return CreateTreeViewItem(diskItemChild);
        }
        #endregion

        #region [Methods]
        public void AddDiskItem(IDiskItem diskItemChild)
        {
            if (diskItemChild != null)
            {
                bool inserted;
                ITreeItem newItem = CreateTreeViewItem(diskItemChild);
                inserted = Insert2SourceLock(newItem);

                if (!inserted)
                    Add2SourceLock(newItem);
            }
        }
        protected override void Add2SourceLock(ITreeItem item)
        {
            if (shower == null)
                base.Add2SourceLock(item);
            else
                shower.Add2Source(item, this);
        }

        protected override void ClearCollectionLock()
        {
            if (shower == null)
                base.ClearCollectionLock();
            else
                shower.Clear(this);
        }

        protected override bool Insert2SourceLock(ITreeItem item)
        {
            if (shower == null)
                return base.Insert2SourceLock(item);
            else
                return shower.Insert2Source(item, this);
        }
        #endregion
    }

    public class TreeDiskItem<T> : TreeDiskItem, IDecorated<T> where T : IDecorator, new()
    {
        #region [Fields]
        private readonly T decorator = new T();
        #endregion

        #region [Properties]
        protected IDecorator Decor { get => decorator; }
        #endregion

        #region [IDecorated<T> Implementation]
        bool IDecorated<T>.Focusable => decorator.Focusable;
        bool IDecorated<T>.ThreeState => decorator.ThreeState;
        #endregion

        #region [Constructors]
        public TreeDiskItem(ITreeItem? parent) : base(parent)
        {
        }
        public TreeDiskItem(IShower? shower, ITreeItem? parent) : base(shower, parent)
        {
        }
        public TreeDiskItem(string path, ITreeItem? parent) : base(path, parent)
        {
        }
        public TreeDiskItem(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeDiskItem(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }
        public TreeDiskItem(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent) { }
        #endregion
    }
}