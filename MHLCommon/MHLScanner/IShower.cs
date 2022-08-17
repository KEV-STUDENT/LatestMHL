using System.Collections.ObjectModel;
using MHLCommon.MHLDiskItems;

namespace MHLCommon.MHLScanner
{
    public interface IShower
    {
        ObservableCollection<ITreeItem> SourceItems { get; }
        void UpdateView();
        void UpdateView(ITreeItem treeViewDiskItem);
        void AddDiskItem(IDiskItem item, ITreeDiskItem parent);
        void LoadItemCollection(ITreeCollectionItem treeItem);
    }
}