using System.IO.Compression;
using MHLSourceOnDisk;
using System.Collections.ObjectModel;
using MHLCommon;
using System.Threading.Tasks.Dataflow;
using MHLCommon.MHLScanner;

namespace MHLSourceScannerModelLib
{
    public class TreeItem : ITreeItem
    {
        protected bool selected = false;
        protected string name = string.Empty;
        protected ITreeItem? parent;

        string ITreeItem.Name => Name;
        public string Name
        {
            get => name;
            set => name = value;
        }

        bool ITreeItem.Selected
        {
            get => Selected;
            set => Selected = value;
        } 
        public bool Selected
        {
            get => selected;
            set => selected = value;
        }

        ITreeItem? ITreeItem.Parent => parent;
        public ITreeItem? Parent
        {
            get => parent;
            set => parent = value;
        }

        #region [Constructors]
        public TreeItem(string name, ITreeItem? parent)
        {
            this.name = name;
            this.parent = parent;
        }

        public TreeItem(ITreeItem? parent) : this(string.Empty, parent)
        {
        }
        #endregion



        #region [TreeItemComparer : Comparer<ITreeItem> implementation]
        int IComparable<ITreeItem>.CompareTo(ITreeItem? other)
        {
            return MHLCommonStatic.CompareStringByLength(this.Name, other?.Name ?? String.Empty);
        }
        #endregion
    }
}