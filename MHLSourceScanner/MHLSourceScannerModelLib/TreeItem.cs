using MHLCommon;
using MHLCommon.MHLScanner;

namespace MHLSourceScannerModelLib
{
    public class TreeItem : ITreeItem
    {
        #region [Fields]
        protected string name = string.Empty;
        protected ITreeItem? parent;
        #endregion

        #region [Properties]
        public string Name
        {
            get => name;
            set => name = value;
        }
        public ITreeItem? Parent
        {
            get => parent;
            set => parent = value;
        }
        #endregion

        #region [ITreeItem Implementation]
        string ITreeItem.Name => Name;
        ITreeItem? ITreeItem.Parent => parent;
        #endregion


        #region [Constructors]

        public TreeItem(string name, ITreeItem parent)
        {
            this.name = name;
            this.parent = parent;
        }

        public TreeItem(ITreeItem parent) : this(string.Empty, parent)
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

    public class TreeItem<T> : TreeItem, IDecorated<T> where T : IDecorator, new()
    {
        #region [Fields]
        private readonly T decorator;
        #endregion

        #region [Properties]
        protected IDecorator Decor { get => decorator; }
        #endregion

        #region [IDecorated<T> Implementation]
        bool IDecorated<T>.Focusable => decorator.Focusable;
        bool IDecorated<T>.ThreeState => decorator.ThreeState;
        #endregion

        #region [Constructors]

        public TreeItem(string name, ITreeItem parent):base(name, parent)
        {
            decorator = new T();
        }

        public TreeItem(ITreeItem parent) : this(string.Empty, parent)
        {
        }
        #endregion
    }
}