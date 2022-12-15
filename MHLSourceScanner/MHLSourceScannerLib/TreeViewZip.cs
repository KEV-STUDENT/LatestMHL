using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceOnDisk;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public class Decor4Zip : Decorator4WPF
    {
        public override Brush ForeGround => Brushes.DarkBlue;
        public override FontWeight FontWeight => FontWeights.Bold;
        public override bool Focusable => true;
        public override bool ThreeState => true;
    }

    public class TreeViewZip : TreeViewDiskItem<Decor4Zip, ViewModel4TreeItem>
    {
        private readonly object sourceLock = new object();

        public int Count
        {
            get
            {
                if (Source != null && Source is IDiskCollection diskCollection)
                {
                    return diskCollection.Count;
                }
                return 0;
            }
        }

        public int TotalCount
        {
            get
            {
                int cnt = 0;
                if (Source != null && Source is IDiskCollection diskCollection)
                {
                    foreach (var item in diskCollection.GetChilds())
                    {
                        if (item is IDiskCollection collection)
                            cnt += collection.Count;
                    }
                }
                return cnt;
            }
        }

        public System.Windows.Visibility Visibility2TotalCount
        {
            get { return (TotalCount < IDiskCollection.MaxItemsInVirtualGroup ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible); }
        }
        #region [Constructors]
        public TreeViewZip(string path, ITreeItem? parent) : base(path, parent)
        {
        }
        public TreeViewZip(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewZip(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }

        public TreeViewZip(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
        #endregion

        #region [Protected Methods]
        protected override void LoadItemCollection(IDiskCollection diskCollection)
        {
            IEnumerable<IDiskItem> itemsList = diskCollection.GetChilds();
            Parallel.ForEach(itemsList, item =>
            {
                ITreeItem newItem = CreateTreeViewItem(item);

                if (!Insert2SourceLock(newItem))
                    Add2SourceLock(newItem);
            });
        }
        #endregion
    }
}

