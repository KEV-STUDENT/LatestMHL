using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceOnDisk;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public class Decor4Ddirectory : Decorator4WPF
    {
        public override Brush ForeGround => Brushes.DarkBlue;
        public override FontWeight FontWeight => FontWeights.Bold;
        public override bool Focusable => true;
        public override bool ThreeState => true;
    }

    public class TreeViewDirectory : TreeViewDiskItem<Decor4Ddirectory>
    {
        #region [Constructors]
        public TreeViewDirectory(string path, ITreeItem? parent) : base(path, parent)
        {
        }
        public TreeViewDirectory(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewDirectory(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }

        public TreeViewDirectory(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
        #endregion

        #region [Protected Methods]
       /* protected override void LoadItemCollection(IDiskCollection diskCollection)
        {
            if ((diskCollection?.Count ?? 0) != 0 && !string.IsNullOrEmpty(Source?.Path2Item)) 
            {
                Parallel.ForEach(diskCollection.GetChildsNames(), name =>
                {
                    IDiskItem diskItemChild;
                    diskItemChild = DiskItemFabrick.GetDiskItem(name);

                    if (diskItemChild != null)
                    {
                        ITreeDiskItem diskItem = this;
                        if (shower == null)
                            diskItem.AddDiskItem(diskItemChild);
                        else
                            shower.AddDiskItem(diskItemChild, diskItem);
                    }
                });
            }
        }*/
        #endregion
    }
}
