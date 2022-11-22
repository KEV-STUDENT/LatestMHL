using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4Ddirectory : IDecorator4WPF
    {
        public Brush ForeGround => Brushes.DarkBlue;
        public FontWeight FontWeight => FontWeights.Bold;
        public bool Focusable => true;
        public bool ThreeState => true;
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
    }
}
