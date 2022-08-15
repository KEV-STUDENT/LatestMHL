using System.Collections.ObjectModel;

namespace MHLCommon
{
    public interface ITreeDiskItem : IComparable<ITreeDiskItem>
    {
        string Path2Item { get; }
        string Name { get; }
        ObservableCollection<ITreeDiskItem> SourceItems { get; protected set; }
        
        void LoadChilds();
        void AddDiskItem(IDiskItem diskItemChild);
        void LoadItemCollection();
        ITreeDiskItem CreateTreeViewItem(IDiskItem diskItemChild);
        ObservableCollection<ITreeDiskItem> LoadChildsCollection();
        Task<ObservableCollection<ITreeDiskItem>> LoadChildsCollectionAsync();
    }
}