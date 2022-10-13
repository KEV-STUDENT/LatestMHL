using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLSourceOnDisk;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;

namespace MHLSourceScannerModelLib
{
    public class TreeDiskItem<T> : TreeCollectionItem<T>, ITreeDiskItem where T:ISelected, new()
    {
        protected readonly IDiskItem? source;
        protected readonly IShower? shower;
        
        private readonly object sourceLock = new object();

        #region [Properties]
        string ITreeDiskItem.Path2Item => source?.Path2Item ?? String.Empty;

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
            source = DiskItemFabrick.GetDiskItem(path);
            InitSourceItems();
        }

        public TreeDiskItem(IDiskItem diskItemSource, ITreeItem? parent) : this(diskItemSource, null, parent)
        {
            source = diskItemSource;
            InitSourceItems();
        }
        public TreeDiskItem(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : this(shower, parent)
        {
            source = diskItemSource;
            InitSourceItems();
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
        private void InitSourceItems()
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
            return new TreeItem<T>(null);
        }

        public virtual ITreeItem CreateTreeViewItem(IDiskItem diskItemChild)
        {
            return new TreeDiskItem<T>(diskItemChild, this);
        }

        protected virtual void AddDiskItem(IDiskItem diskItemChild)
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
}