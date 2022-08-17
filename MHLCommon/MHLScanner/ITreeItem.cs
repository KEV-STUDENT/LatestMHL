using System.Collections.ObjectModel;
using MHLCommon.MHLDiskItems;

namespace MHLCommon.MHLScanner
{
    public interface ITreeItem : IComparable<ITreeItem>
    {
        string Name { get; }
    }

    public interface ITreeCollectionItem : ITreeItem
    {
        ObservableCollection<ITreeItem> SourceItems { get; protected set; }
        void LoadItemCollection();
        void LoadChilds();
        ObservableCollection<ITreeItem> LoadChildsCollection();
        Task<ObservableCollection<ITreeItem>> LoadChildsCollectionAsync();
    }

    public interface ITreeDiskItem : ITreeCollectionItem
    {
        string Path2Item { get; }

        void AddDiskItem(IDiskItem diskItemChild);

        ITreeItem CreateTreeViewItem(IDiskItem diskItemChild);
    }
}