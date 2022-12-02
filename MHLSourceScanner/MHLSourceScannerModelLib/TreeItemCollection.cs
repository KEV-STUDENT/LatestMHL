using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MHLSourceScannerModelLib
{
    public class TreeItemCollection: TreeItem, ITreeItemCollection
    {

        private bool childsLoaded;
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
        #endregion

        #region [ITreeCollectionItem implementation]
        ObservableCollection<ITreeItem> ITreeItemCollection.SourceItems
        {
            get => SourceItems;
            set => SourceItems = value;
        }
        void ITreeItemCollection.LoadChilds()
        {
            if (childsLoaded)
                return;

            LoadChilds();

            childsLoaded = true;
        }
        void ITreeItemCollection.LoadItemCollection()
        {
            LoadItemCollection();
        }

        ObservableCollection<ITreeItem> ITreeItemCollection.LoadChildsCollection()
        {
            return LoadChildsCollection();
        }

        async Task<ObservableCollection<ITreeItem>> ITreeItemCollection.LoadChildsCollectionAsync()
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
