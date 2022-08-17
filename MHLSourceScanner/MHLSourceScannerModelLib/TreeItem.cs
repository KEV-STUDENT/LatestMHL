using System.IO.Compression;
using MHLSourceOnDisk;
using System.Collections.ObjectModel;
using MHLCommon;
using System.Threading.Tasks.Dataflow;
using MHLCommon.MHLScanner;

namespace MHLSourceScannerModelLib
{
    public abstract class TreeItem : ITreeCollectionItem
    {
        private bool childsLoaded;
        protected readonly ITreeCollectionItem treeItem;
        protected string name = string.Empty;
        private ObservableCollection<ITreeItem> sourceItems = new ObservableCollection<ITreeItem>();

        public ObservableCollection<ITreeItem> SourceItems
        {
            get => sourceItems;
            set => sourceItems = value;

        }
        ObservableCollection<ITreeItem> ITreeCollectionItem.SourceItems
        {
            get => SourceItems;
            set => SourceItems = value;
        }
        string ITreeItem.Name => name;

        #region [Constructors]
        public TreeItem()
        {
            treeItem = this;
        }
        #endregion

        #region [ITreeItem implementation]
        void ITreeCollectionItem.LoadChilds()
        {
            if (childsLoaded)
                return;

            LoadChilds();

            childsLoaded = true;
        }
        void ITreeCollectionItem.LoadItemCollection()
        {
            LoadItemCollection();
        }

        ObservableCollection<ITreeItem> ITreeCollectionItem.LoadChildsCollection()
        {
            return LoadChildsCollection();
        }

        async Task<ObservableCollection<ITreeItem>> ITreeCollectionItem.LoadChildsCollectionAsync()
        {
            System.Diagnostics.Debug.WriteLine("Thread 1 : {0}  Task : {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
            return await Task<ObservableCollection<ITreeDiskItem>>.Run(() => treeItem.LoadChildsCollection());
        }
        #endregion

        #region [TreeItemComparer : Comparer<ITreeItem> implementation]
        int IComparable<ITreeItem>.CompareTo(ITreeItem? other)
        {
            return MHLCommonStatic.CompareStringByLength(treeItem.Name, other?.Name ?? String.Empty);
        }
        #endregion

        public virtual void LoadChilds()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadItemCollection()
        {
            throw new NotImplementedException();
        }

        public virtual ObservableCollection<ITreeItem> LoadChildsCollection()
        {
            throw new NotImplementedException();
        }
    }
}