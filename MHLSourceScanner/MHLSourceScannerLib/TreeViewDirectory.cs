using MHLCommon;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4Ddirectory : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.DarkBlue;
        FontWeight IDecorator.FontWeight => FontWeights.Bold;
        bool IDecorator.Focusable => true;
    }

    public class TreeViewDirectory : TreeViewDiskItem<Decor4Ddirectory>
    {
        #region [Constructors]
        public TreeViewDirectory(string path) : base(path)
        {
        }
        public TreeViewDirectory(string path, IShower? shower) : base(path, shower)
        {
        }

        public TreeViewDirectory(IDiskItem diskItemSource) : base(diskItemSource)
        {
        }

        public TreeViewDirectory(IDiskItem diskItemSource, IShower? shower) : base(diskItemSource, shower)
        {
        }
        #endregion
    }
}
