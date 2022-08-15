using MHLCommon;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4FB2Attributes : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.DarkGreen;
        FontWeight IDecorator.FontWeight => FontWeights.Bold;
        bool IDecorator.Focusable => true;
    }

    public class TreeViewFB2Attributes : TreeViewDiskItem<Decor4FB2Attributes>
    {
        public string Title
        {
            get
            {
                if (source != null && source is IBook book)
                {
                    return book.Title;
                }
                return string.Empty;
            }
        }
        #region [Constructors]
        public TreeViewFB2Attributes(string path) : base(path)
        {
        }
        public TreeViewFB2Attributes(string path, IShower? shower) : base(path, shower)
        {
        }

        public TreeViewFB2Attributes(IDiskItem diskItemSource) : base(diskItemSource)
        {
        }

        public TreeViewFB2Attributes(IDiskItem diskItemSource, IShower? shower) : base(diskItemSource, shower)
        {
        }
        #endregion

        #region [Protected Methods]
        protected override void LoadItemCollection(IDiskCollection diskCollection)
        {
            base.LoadItemCollection(diskCollection);
        }
        #endregion
    }
}

