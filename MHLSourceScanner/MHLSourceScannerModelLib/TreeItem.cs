using MHLCommon;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;

namespace MHLSourceScannerModelLib
{
    public class TreeItem<T> : ITreeItem where T : ISelected, new()
    {
        protected string name = string.Empty;
        protected ITreeItem? parent;
        private T viewModel;


        string ITreeItem.Name => Name;
        public string Name
        {
            get => name;
            set => name = value;
        }

        bool ITreeItem.Selected
        {
            get => viewModel.IsSelected;
            set
            {
                viewModel.IsSelected = value;
                if (parent != null)
                    parent.Selected = value;                
            }
        }

        ITreeItem? ITreeItem.Parent => parent;
        public ITreeItem? Parent
        {
            get => parent;
            set => parent = value;
        }

        public T ViewModel
        {
            get => viewModel;
        }

        #region [Constructors]
        public TreeItem(string name, ITreeItem? parent)
        {
            this.name = name;
            this.parent = parent;
            viewModel = new T();
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