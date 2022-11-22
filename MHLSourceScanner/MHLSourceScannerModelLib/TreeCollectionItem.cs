using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System.Collections.ObjectModel;

namespace MHLSourceScannerModelLib
{
    public class TreeCollectionItem: TreeItem, ITreeCollectionItem
    {

        private bool childsLoaded;
        private ObservableCollection<ITreeItem> sourceItems = new ObservableCollection<ITreeItem>();

        #region [Constructors]
        public TreeCollectionItem(string name, ITreeItem parent) : base(name, parent) { }
        public TreeCollectionItem(ITreeItem parent) : base(parent) { }
        #endregion

        #region [Properties]
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
        #endregion

        #region [ITreeCollectionItem implementation]
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
            return await Task<ObservableCollection<ITreeDiskItem>>.Run(() => LoadChildsCollection());
        }
        #endregion

        #region [Methods]
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
        #endregion
    }
}
