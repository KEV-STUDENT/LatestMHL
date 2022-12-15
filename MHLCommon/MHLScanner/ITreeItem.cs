using System.Collections.ObjectModel;
using System.ComponentModel;
using MHLCommon.MHLDiskItems;

namespace MHLCommon.MHLScanner
{

    public interface IItemSelected
    {
        bool? Selected { get; set; }
    }

    public interface ITreeItem : IComparable<ITreeItem>
    {
        ITreeItem? Parent { get; }
        string Name { get; }

        IDiskItem? Source{get;set;}
    }

    public interface IDecorated<T> where T : IDecorator, new()
    {
        bool Focusable { get; }
        bool ThreeState { get; }
    }

    public interface ITreeItemCollection : ITreeItem
    {
        ObservableCollection<ITreeItem> SourceItems { get; set; }
        bool ChildsLoaded { get; }

        //void LoadItemCollection();
        //void LoadChilds();
        //ObservableCollection<ITreeItem> LoadChildsCollection();
        //Task<ObservableCollection<ITreeItem>> LoadChildsCollectionAsync();
        Task LoadChildsAsync();
        void Add2Source(ITreeItem item);
        void ClearCollection();
        bool Insert2Source(ITreeItem item);
    }

    public interface ITreeDiskItem : ITreeItemCollection
    {
        string Path2Item { get; }

        void AddDiskItem(IDiskItem diskItemChild);

        ITreeItem CreateTreeViewItem(IDiskItem diskItemChild);
    }
}