using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLSourceOnDisk;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Documents;

namespace MHLSourceScannerModelLib
{
    public class TreeItemCollection : TreeItem, ITreeItemCollection
    {
        private bool processLoadEnabled = true;
        private bool childsLoaded;
        private readonly object sourceLock = new object();
        private ObservableCollection<ITreeItem> sourceItems = new ObservableCollection<ITreeItem>();

        #region [Constructors]
        public TreeItemCollection(string name, ITreeItem? parent) : base(name, parent) { }
        public TreeItemCollection(string name) : base(name) { }
        public TreeItemCollection(ITreeItem? parent) : base(parent) { }
        public TreeItemCollection() : base() { }
        #endregion

        #region [Properties]
        [JsonIgnore]
        public ObservableCollection<ITreeItem> SourceItems
        {
            get => sourceItems;
            set => sourceItems = value;

        }

        public bool ChildsLoaded
        {
            get => childsLoaded;
            set => childsLoaded = value;
        }
        #endregion

        #region [ITreeCollectionItem implementation]
        ObservableCollection<ITreeItem> ITreeItemCollection.SourceItems
        {
            get => SourceItems;
            set => SourceItems = value;
        }
        bool ITreeItemCollection.ChildsLoaded => ChildsLoaded;
        
        async Task ITreeItemCollection.LoadChildsAsync()
        {
            if (processLoadEnabled)
            {
                processLoadEnabled = false;
                await LoadChildsAsync().ContinueWith((t) => { processLoadEnabled = t.IsCompleted; });
            }
        }

       void ITreeItemCollection.Add2Source(ITreeItem item)
        {
            Add2Source(item);
        }

        void ITreeItemCollection.ClearCollection()
        {
            ClearCollection();
        }

        bool ITreeItemCollection.Insert2Source(ITreeItem item)
        {
            return Insert2Source(item);
        }
        #endregion

        #region [Methods]
        public virtual void LoadChilds4Collection()
        {
            ClearCollectionLock();
            LoadItemCollection();
        }

        public virtual void LoadItemCollection()
        {
            if (Source is IDiskCollection diskCollection)
            {
                LoadItemCollection(diskCollection);
            }
        }

        protected virtual void LoadItemCollection(IDiskCollection diskCollection)
        {
            if ((diskCollection?.Count ?? 0) != 0)
            {
                int i = 0;
                IEnumerable<string> names = diskCollection.GetChildsNames();

                ParallelOptions options = new ParallelOptions();
                options.MaxDegreeOfParallelism = 3;

                Parallel.ForEach(names, options, (itemName, state) =>
                {
                    IDiskItem diskItemChild = DiskItemFabrick.GetDiskItem(itemName);
                    ITreeItem newItem = CreateTreeViewItem(diskItemChild);
                   
                    if (!Insert2SourceLock(newItem))
                        Add2SourceLock(newItem);                  
                });
            }
        }

        public virtual ITreeItem CreateEmptyItem()
        {
            return new TreeItem();
        }

        public virtual ITreeItem CreateTreeViewItem(IDiskItem diskItemChild)
        {
            return new TreeDiskItem(diskItemChild, this);
        }

        public void ClearCollection()
        {
            lock (sourceLock)
            {
                SourceItems.Clear();
            }
        }

        public void Add2Source(ITreeItem item)
        {
            lock (sourceLock)
            {
                SourceItems.Add(item);
            }
        }

        protected override void InitSourceItems()
        {
            base.InitSourceItems();
            if (Source is IDiskCollection diskCollection)
            {
                if (diskCollection.Count > 0)
                {
                    SourceItems.Add(CreateEmptyItem());
                }
            }
            else if (Source is IMHLBook)
                SourceItems.Add(CreateEmptyItem());
        }

        protected virtual void ClearCollectionLock()
        {
            ClearCollection();
        }

        protected virtual void Add2SourceLock(ITreeItem item)
        {
            Add2Source(item);
        }

        protected virtual bool Insert2SourceLock(ITreeItem item)
        {
           return Insert2Source(item);
        }

        protected bool Insert2Source(ITreeItem item)
        {
            bool inserted = false;
            lock (sourceLock)
            {
                for (int i = 0; i < SourceItems.Count; i++)
                {
                    if (MHLCommonStatic.CompareStringByLength(SourceItems[i].Name, item.Name) > 0)
                    {
                        SourceItems.Insert(i, item);
                        System.Diagnostics.Debug.WriteLine("Inserted : {0} Position {1}", item.Name, i);
                        inserted = true;
                        break;
                    }
                }
            }

            return inserted;
        }

        protected virtual void LoadChilds()
        {
            if (childsLoaded)
                return;

            LoadChilds4Collection();
            childsLoaded = true;
        }

        private async Task LoadChildsAsync()
        {
            await Task.Factory.StartNew(LoadChilds);
        }
        #endregion
    }
}
