using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace MHLSourceScannerModelLib
{
    public class TreeItem : ITreeItem
    {
        #region [Fields]
        protected string name = string.Empty;
        protected ITreeItem? parent;
        private IDiskItem? source = null;
        #endregion

        #region [Properties]
        [JsonIgnore]
        public string Name
        {
            get => name;
            set => name = value;
        }
        [JsonIgnore]
        public ITreeItem? Parent
        {
            get => parent;
            set => parent = value;
        }
        public IDiskItem? Source
        {
            get => source;
            set{
                source = value;
                InitSourceItems();
            }
        }
        #endregion

        #region [ITreeItem Implementation]
        string ITreeItem.Name => Name;
        ITreeItem? ITreeItem.Parent => parent;

        IDiskItem? ITreeItem.Source { get => Source; set => Source = value; }
        #endregion


        #region [Constructors]
        public TreeItem(string name, ITreeItem? parent) : base()
        {
            this.name = name;
            this.parent = parent;
        }

        public TreeItem(string name) : this(name, null) { }

        public TreeItem(ITreeItem? parent) : this(string.Empty, parent) { }

        public TreeItem() : this(string.Empty, null) { }
        #endregion

        #region [TreeItemComparer : Comparer<ITreeItem> implementation]
        int IComparable<ITreeItem>.CompareTo(ITreeItem? other)
        {
            return MHLCommonStatic.CompareStringByLength(this.Name, other?.Name ?? String.Empty);
        }
        #endregion

        #region [Private Methods]
        protected virtual void InitSourceItems()
        {
            Name = Source?.Name ?? String.Empty;
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

        public TreeItem(string name, ITreeItem parent) : base(name, parent)
        {
            decorator = new T();
        }

        public TreeItem(ITreeItem parent) : this(string.Empty, parent)
        {
        }
        #endregion
    }
}