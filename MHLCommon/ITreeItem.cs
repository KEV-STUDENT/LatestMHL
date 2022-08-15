using System.Collections.ObjectModel;

namespace MHLCommon
{
    public interface ITreeItem : IComparable<ITreeItem>
    {
        string Path2Item { get; }
        string Name { get; }
        ObservableCollection<ITreeItem> SourceItems { get; protected set; }
        
        void LoadChilds();
        void AddDiskItem(IDiskItem diskItemChild);
        void LoadItemCollection();
        ITreeItem CreateTreeViewItem(IDiskItem diskItemChild);
        ObservableCollection<ITreeItem> LoadChildsCollection();
        Task<ObservableCollection<ITreeItem>> LoadChildsCollectionAsync();
    }
}