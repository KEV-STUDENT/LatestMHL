using MHLSourceOnDisk;
using System.Collections.ObjectModel;
using MHLCommon;

namespace MHLSourceScannerModelLib
{
    public class DiskItemShower : IShower
    {
        public Action? BeforeUpdateView;
        public Action? AfterUpdateView;
        public Action? UpdateView;
        public Action<IDiskItem, ITreeItem>? CreateItem;
        public Action<ITreeCollectionItem>? LoadCollection;    
    
        protected ObservableCollection<ITreeItem> sourceItems = new ObservableCollection<ITreeItem>();

        public ObservableCollection<ITreeItem> SourceItems
        {
            get { return sourceItems; }
        }

        ObservableCollection<ITreeItem> IShower.SourceItems {
            get { return SourceItems; }
        }

        void IShower.LoadItemCollection(ITreeCollectionItem treeItem)
        {
            if(LoadCollection == null)
                treeItem.LoadItemCollection();
            else
                LoadCollection(treeItem);            
        }

        void IShower.AddDiskItem(IDiskItem item, ITreeDiskItem parent)
        {            
            if(CreateItem == null)
                parent.AddDiskItem(item);
            else
                CreateItem(item, parent);
        }

        void IShower.UpdateView()
        {
            if(BeforeUpdateView != null)
                BeforeUpdateView();

            if(UpdateView != null)
                UpdateView();

            if(AfterUpdateView != null)
                AfterUpdateView();
        }

        void IShower.UpdateView(ITreeItem treeViewItem)
        {
            if (treeViewItem is ITreeDiskItem treeViewDiskItem)
            {
                treeViewDiskItem.LoadChilds();
                sourceItems = treeViewDiskItem.SourceItems;
            }
            ((IShower)this).UpdateView();
        }
    }
}