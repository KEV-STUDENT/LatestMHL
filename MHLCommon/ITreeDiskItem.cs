using System.Collections.ObjectModel;

namespace MHLCommon
{
    public interface ITreeItem : IComparable<ITreeItem>
    {
        string Name { get; }
        void LoadItemCollection();
        void LoadChilds();
        ObservableCollection<ITreeItem> LoadChildsCollection();
        Task<ObservableCollection<ITreeItem>> LoadChildsCollectionAsync();
    }

     public interface ITreeDiskItem : ITreeItem
    {
        string Path2Item { get; }
       
        ObservableCollection<ITreeDiskItem> SourceItems { get; protected set; }
        
       
        void AddDiskItem(IDiskItem diskItemChild);
        
        ITreeDiskItem CreateTreeViewItem(IDiskItem diskItemChild);       
    }
}