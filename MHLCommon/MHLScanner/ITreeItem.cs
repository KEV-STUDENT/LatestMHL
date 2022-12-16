using MHLCommon.MHLDiskItems;
using System.Collections.ObjectModel;

namespace MHLCommon.MHLScanner
{

    public interface IItemSelected
    {
        bool? Selected { get; set; }
        bool IsExported { get; set; }
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
        Task ExportItemAsync(IExport exporter);
    }
}