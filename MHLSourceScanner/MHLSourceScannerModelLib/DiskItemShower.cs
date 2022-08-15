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
        public Action<IDiskItem, ITreeDiskItem>? CreateItem;
        public Action<ITreeDiskItem>? LoadCollection;    
    
        protected ObservableCollection<ITreeDiskItem> sourceItems = new ObservableCollection<ITreeDiskItem>();

        public ObservableCollection<ITreeDiskItem> SourceItems
        {
            get { return sourceItems; }
        }

        ObservableCollection<ITreeDiskItem> IShower.SourceItems {
            get { return SourceItems; }
        }

        void IShower.LoadItemCollection(ITreeDiskItem treeItem)
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

        void IShower.UpdateView(ITreeDiskItem treeViewDiskItem)
        {
            if (treeViewDiskItem != null)
            {
                treeViewDiskItem.LoadChilds();
                sourceItems = treeViewDiskItem.SourceItems;
            }
            ((IShower)this).UpdateView();
        }
    }
}