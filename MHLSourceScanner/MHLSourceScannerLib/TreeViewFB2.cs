using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4FB2 : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.DarkGreen;
        FontWeight IDecorator.FontWeight => FontWeights.Bold;
        bool IDecorator.Focusable => true;
        bool IDecorator.ThreeState => false;

    }

    public class TreeViewFB2 : TreeViewDiskItem<Decor4FB2>
    {
        #region [Properties]
        public string Title
        {
            get
            {
                if (source is IBook book)
                {
                    return book.Title;
                }
                return string.Empty;
            }
        }

        public string Cover
        {
            get
            {
                if(source is IBook book)
                    return book.Cover;
                return string.Empty;
            }
        }

        public IBook? Book { get { return source as IBook; } }
        #endregion

        #region [Constructors]
        public TreeViewFB2(string path, ITreeItem? parent) : base(path, parent)
        {
        }
        public TreeViewFB2(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewFB2(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }

        public TreeViewFB2(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
        #endregion

        #region [Protected Methods]
        public override void LoadItemCollection()
        {
            if (source is IBook book)
            {
               /* if (!string.IsNullOrEmpty(book.Annotation))
                    SourceItems.Add(new FB2Annotation(book.Annotation));*/

                if (book.Authors.Count > 0)
                    SourceItems.Add(new FB2Authors(book, this));

                if (book.Genres.Count > 0)
                    SourceItems.Add(new FB2Genres(book, this));

                if (book.Keywords.Count > 0)
                    SourceItems.Add(new FB2Keywords(book, this));
            }
        }
        #endregion
    }
}

