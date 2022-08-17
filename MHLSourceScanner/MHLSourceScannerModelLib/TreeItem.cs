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
        protected string name = string.Empty;

        string ITreeItem.Name => Name;
        public string Name
        {
            get => name;
            set => name = value;
        }

        #region [Constructors]
        public TreeItem(string name)
        {
            this.name = name;
        }

        public TreeItem() : this(string.Empty)
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