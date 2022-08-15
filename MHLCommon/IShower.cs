using System.Collections.ObjectModel;
namespace MHLCommon
{
    public interface IShower
    {
        ObservableCollection<ITreeDiskItem> SourceItems { get; }
        void UpdateView();
        void UpdateView(ITreeDiskItem treeViewDiskItem);
        void AddDiskItem(IDiskItem item, ITreeDiskItem parent);
        void LoadItemCollection(ITreeDiskItem treeItem);
    }
}