using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLSourceOnDisk;
using System.Collections.ObjectModel;

namespace MHLSourceScannerModelLib
{
    public class TreeDiskItem : TreeItemCollection, ITreeDiskItem
    {
        protected IDiskItem? source;
        protected IShower? shower;

        private readonly object sourceLock = new object();

        #region [Properties]
        public string Path2Item => source?.Path2Item ?? String.Empty;
        string ITreeDiskItem.Path2Item => Path2Item;
        public IShower? Shower { get => shower; set => shower = value; }
        public IDiskItem? Source
        {
            get => source;
            set
            {
                source = value;
                InitSourceItems();
            }
        }
        public bool TestMode { get; set; }
        #endregion

        #region [Constructors]
        public TreeDiskItem(ITreeItem? parent) : base(parent)
        {
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

        #region [TreeItem implementation]
        public override void LoadChilds()
        {
            if (shower == null)
                LoadItemCollection();
            else
                shower.LoadItemCollection(this);
        }

        public override void LoadItemCollection()
        {
            if (source is IDiskCollection diskCollection)
            {
                SourceItems.Clear();
                LoadItemCollection(diskCollection);
            }
        }

        public override ObservableCollection<ITreeItem> LoadChildsCollection()
        {
            ObservableCollection<ITreeItem> res = new ObservableCollection<ITreeItem>();
            if (source != null && source is IDiskCollection diskCollection)
            {
                foreach (IDiskItem diskItemChild in diskCollection.GetChilds())
                {
                    res.Add(CreateTreeViewItem(diskItemChild));
                }
            }
            return res;
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

        #region [Private Methods]
        protected virtual void InitSourceItems()
        {
            if (source is IDiskCollection diskCollection)
            {
                if (diskCollection.Count > 0)
                {
                    SourceItems.Add(CreateEmptyItem());
                }
            }
            else if (source is IBook)
                SourceItems.Add(CreateEmptyItem());

            name = source?.Name ?? String.Empty;
        }
        #endregion

        #region [Protected Methods]
        protected virtual void LoadItemCollection(IDiskCollection diskCollection)
        {
            if ((diskCollection?.Count ?? 0) != 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (IDiskItem diskItemChild in diskCollection.GetChilds())
                {
                    tasks.Add(Task.Run(() =>
                    {
                        if (shower == null)
                            ((ITreeDiskItem)this).AddDiskItem(diskItemChild);
                        else
                            shower.AddDiskItem(diskItemChild, this);
                    }));
                }
                if (TestMode)
                    Task.WaitAll(tasks.ToArray());
            }
        }
        #endregion

        #region [Public Methods]
        public virtual ITreeItem CreateEmptyItem()
        {
            return new TreeItem();
        }

        public virtual ITreeItem CreateTreeViewItem(IDiskItem diskItemChild)
        {
            return new TreeDiskItem(diskItemChild, this);
        }

        public virtual void AddDiskItem(IDiskItem diskItemChild)
        {
            if (diskItemChild != null)
            {
                bool inserted = false;
                ITreeItem newItem = CreateTreeViewItem(diskItemChild);
                lock (sourceLock)
                {
                    for (int i = 0; i < SourceItems.Count; i++)
                    {
                        if (MHLCommonStatic.CompareStringByLength(SourceItems[i].Name, diskItemChild.Name) > 0)
                        {
                            SourceItems.Insert(i, newItem);
                            inserted = true;
                            break;
                        }
                    }

                    if (!inserted)
                        SourceItems.Add(newItem);
                }
            }
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