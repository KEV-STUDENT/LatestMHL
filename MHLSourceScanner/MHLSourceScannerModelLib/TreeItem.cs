using System.IO.Compression;
using MHLSourceOnDisk;
using System.Collections.ObjectModel;
using MHLCommon;

namespace MHLSourceScannerModelLib
{
    public class TreeItem : ITreeDiskItem
    {
        protected readonly IDiskItem? source;
        protected readonly IShower? shower;
        private bool childsLoaded;
        private ObservableCollection<ITreeDiskItem> sourceItems = new ObservableCollection<ITreeDiskItem>();
        protected readonly ITreeDiskItem treeItem;

        private readonly object sourceLock = new object();

        #region [Properties]
        public ObservableCollection<ITreeDiskItem> SourceItems
        {
            get => sourceItems;
            set => sourceItems = value;

        }
        ObservableCollection<ITreeDiskItem> ITreeDiskItem.SourceItems
        {
            get => SourceItems;
            set => SourceItems = value;
        }

        public string Name
        {
            get => source?.Name ?? String.Empty;
        }
        string ITreeDiskItem.Path2Item => source?.Path2Item ?? String.Empty;
        string ITreeItem.Name => Name;

        public bool TestMode { get; set; }
        #endregion

        #region [Constructors]
        public TreeItem()
        {
            treeItem = this;
        }
        public TreeItem(IShower? shower) : this()
        {
            this.shower = shower;
        }

        public TreeItem(string path) : this(path, null)
        {
        }
        public TreeItem(string path, IShower? shower) : this(shower)
        {
            source = DiskItemFabrick.GetDiskItem(path);
            InitSourceItems();
        }

        public TreeItem(IDiskItem diskItemSource) : this(diskItemSource, null)
        {
            source = diskItemSource;
            InitSourceItems();
        }
        public TreeItem(IDiskItem diskItemSource, IShower? shower) : this(shower)
        {
            source = diskItemSource;
            InitSourceItems();
        }
        #endregion

        #region [ITreeItem implementation]
        void ITreeItem.LoadChilds()
        {
            if (childsLoaded)
                return;

            if (shower == null)
                treeItem.LoadItemCollection();
            else
                shower.LoadItemCollection(treeItem);

            childsLoaded = true;
        }

        void ITreeItem.LoadItemCollection()
        {
            if (source != null && source is IDiskCollection diskCollection)
            {
                treeItem.SourceItems.Clear();
                LoadItemCollection(diskCollection);
            }
        }

        void ITreeDiskItem.AddDiskItem(IDiskItem diskItemChild)
        {
            if (diskItemChild != null)
            {
                bool inserted = false;
                ITreeDiskItem newItem = treeItem.CreateTreeViewItem(diskItemChild);
                lock (sourceLock)
                {
                    for (int i = 0; i < treeItem.SourceItems.Count; i++)
                    {
                        if (MHLCommonStatic.CompareStringByLength(treeItem.SourceItems[i].Name, diskItemChild.Name) > 0)
                        {
                            treeItem.SourceItems.Insert(i, newItem);
                            inserted = true;
                            break;
                        }
                    }

                    if (!inserted)
                        treeItem.SourceItems.Add(newItem);
                }
            }
        }

        ITreeDiskItem ITreeDiskItem.CreateTreeViewItem(IDiskItem diskItemChild)
        {
            return CreateTreeViewItem(diskItemChild);
        }

        ObservableCollection<ITreeItem> ITreeItem.LoadChildsCollection()
        {
            System.Diagnostics.Debug.WriteLine("Thread 2 : {0}  Task : {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
            ObservableCollection<ITreeItem> res = new ObservableCollection<ITreeItem>();
            if (source != null && source is IDiskCollection diskCollection)
            {
                foreach (IDiskItem diskItemChild in diskCollection.GetChilds())
                {
                    res.Add(treeItem.CreateTreeViewItem(diskItemChild));
                }
            }
            return res;
        }

        async Task<ObservableCollection<ITreeItem>> ITreeItem.LoadChildsCollectionAsync()
        {
            System.Diagnostics.Debug.WriteLine("Thread 1 : {0}  Task : {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
            return await Task<ObservableCollection<ITreeDiskItem>>.Run(() => treeItem.LoadChildsCollection());
        }

        int IComparable<ITreeItem>.CompareTo(ITreeItem? other)
        {
            return MHLCommonStatic.CompareStringByLength(this.Name, other?.Name ?? String.Empty);
        }
        #endregion

        #region [Private Methods]
        private void InitSourceItems()
        {
            if (source is IDiskCollection diskCollection)
                if (diskCollection.Count > 0)
                {
                    sourceItems.Add(CreateEmptyItem());
                }
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
                            treeItem.AddDiskItem(diskItemChild);
                        else
                            shower.AddDiskItem(diskItemChild, treeItem);
                    }));
                }
                if (TestMode)
                    Task.WaitAll(tasks.ToArray());
            }
        }
        #endregion

        #region [Public Methods]
        public virtual ITreeDiskItem CreateEmptyItem()
        {
            return new TreeItem();
        }

        public virtual ITreeDiskItem CreateTreeViewItem(IDiskItem diskItemChild)
        {
            return new TreeItem(diskItemChild);
        }
        #endregion

        #region [TreeItemComparer : Comparer<ITreeItem> implementation]
        /*private class TreeItemComparer : Comparer<ITreeItem>
        {
            public override int Compare(ITreeItem? x, ITreeItem? y)
            {
                return MHLCommonStatic.CompareStringByLength(x?.Name, y?.Name);
            }
        }*/
        #endregion
    }
}