using MHLCommon;
using MHLSourceScannerModelLib;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4FB2 : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.DarkGreen;
        FontWeight IDecorator.FontWeight => FontWeights.Bold;
        bool IDecorator.Focusable => true;
    }

    public class TreeViewFB2 : TreeViewDiskItem<Decor4FB2>
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
        public TreeViewFB2(string path) : base(path)
        {
        }
        public TreeViewFB2(string path, IShower? shower) : base(path, shower)
        {
        }

        public TreeViewFB2(IDiskItem diskItemSource) : base(diskItemSource)
        {
        }

        public TreeViewFB2(IDiskItem diskItemSource, IShower? shower) : base(diskItemSource, shower)
        {
        }
        #endregion

        #region [Protected Methods]
        public override void LoadItemCollection()
        {
            SourceItems.Insert(0, new TreeViewFB2Section() { Name = "Authors" });
            SourceItems.Insert(1, new TreeViewFB2Section() { Name = "Genres" });
            SourceItems.Insert(2, new TreeViewFB2Section() { Name = "Keywords" });
        }
        #endregion
    }
}

