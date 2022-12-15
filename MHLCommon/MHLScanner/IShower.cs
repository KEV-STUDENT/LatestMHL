using System.Collections.ObjectModel;
using MHLCommon.MHLDiskItems;

namespace MHLCommon.MHLScanner
{
    public interface IShower
    {
        ObservableCollection<ITreeItem> SourceItems { get; }
        void UpdateView(ITreeItem treeViewDiskItem);
        void AddDiskItem(IDiskItem item, ITreeDiskItem parent);
        void Clear(ITreeItemCollection treeItem);
        void Add2Source(ITreeItem item, ITreeItemCollection treeView);
        bool Insert2Source(ITreeItem item, ITreeItemCollection treeDiskItem);
    }
}