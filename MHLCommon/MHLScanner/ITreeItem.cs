using System.Collections.ObjectModel;
using System.ComponentModel;
using MHLCommon.MHLDiskItems;

namespace MHLCommon.MHLScanner
{
    public interface ITreeItem : IComparable<ITreeItem>
    {
        ITreeItem? Parent { get; }
        string Name { get; }
        bool Selected { get; set; }
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