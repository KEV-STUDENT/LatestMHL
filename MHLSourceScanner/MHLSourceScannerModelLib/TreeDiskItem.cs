using MHLCommon;
using MHLSourceOnDisk;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;

namespace MHLSourceScannerModelLib
{
    public class TreeDiskItem : TreeItem, ITreeDiskItem
    {
        protected readonly IDiskItem? source;
        protected readonly IShower? shower;
        
        private readonly object sourceLock = new object();

        #region [Properties]
        public string Name
        {
            get => source?.Name ?? String.Empty;
        }
        string ITreeDiskItem.Path2Item => source?.Path2Item ?? String.Empty;
        string ITreeItem.Name
        {
            get => name;
            set => name = value;
        }

        public bool TestMode { get; set; }
        #endregion

        #region [Constructors]
        public TreeDiskItem() : base()
        {
        }
        public TreeDiskItem(IShower? shower) : this()
        {
            this.shower = shower;
        }

        public TreeDiskItem(string path) : this(path, null)
        {
        }
        public TreeDiskItem(string path, IShower? shower) : this(shower)
        {
            source = DiskItemFabrick.GetDiskItem(path);
            InitSourceItems();
        }

        public TreeDiskItem(IDiskItem diskItemSource) : this(diskItemSource, null)
        {
            source = diskItemSource;
            InitSourceItems();
        }
        public TreeDiskItem(IDiskItem diskItemSource, IShower? shower) : this(shower)
        {
            source = diskItemSource;
            InitSourceItems();
        }
        #endregion

        #region [TreeItem implementation]
        public override void LoadChilds()
        {
            if (shower == null)
                treeItem.LoadItemCollection();
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
            System.Diagnostics.Debug.WriteLine("Thread 2 : {0}  Task : {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
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
        public virtual ITreeDiskItem CreateEmptyItem()
        {
            return new TreeDiskItem();
        }

        public virtual ITreeItem CreateTreeViewItem(IDiskItem diskItemChild)
        {
            return new TreeDiskItem(diskItemChild);
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