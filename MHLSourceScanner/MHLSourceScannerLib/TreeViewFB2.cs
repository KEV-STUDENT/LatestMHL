using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public class Decor4FB2 : Decorator4WPF
    {
        public  override Brush ForeGround => Brushes.DarkGreen;
        public override FontWeight FontWeight => FontWeights.Bold;
        public override bool Focusable => true;
    }

    public class TreeViewFB2 : TreeViewDiskItem<Decor4FB2, ViewModel4TreeItem>
    {
        #region [Fields]
        private MHLSequenceNum? _sequenceNum = null;
        private bool _sequenceNumLoaded = false;
        #endregion

        #region [Properties]
        public string Title
        {
            get
            {
                if (Source is IMHLBook book)
                {
                    return book.Title;
                }
                return string.Empty;
            }
        }

        public Visibility SequenceVisibility
        {
            get
            {
                if (!_sequenceNumLoaded)
                {
                    if (Source is IMHLBook book)
                        _sequenceNum = (book.SequenceAndNumber.FirstOrDefault() as MHLSequenceNum);

                    _sequenceNumLoaded = true;
                }
                return (_sequenceNum == null || string.IsNullOrEmpty(_sequenceNum?.Name) ? Visibility.Collapsed : Visibility.Visible) ;
            }
        }

        public string Sequence
        {
            get
            {
                if (!_sequenceNumLoaded)
                {
                    if (Source is IMHLBook book)
                        _sequenceNum = (book.SequenceAndNumber.FirstOrDefault() as MHLSequenceNum);

                    _sequenceNumLoaded = true;
                }
                return _sequenceNum?.Name ?? string.Empty;
            }
        }

        public ushort? Number
        {
            get
            {
                if (!_sequenceNumLoaded)
                {
                    if (Source is IMHLBook book)
                        _sequenceNum = (book.SequenceAndNumber.FirstOrDefault() as MHLSequenceNum);

                    _sequenceNumLoaded = true;
                }

                return _sequenceNum?.Number;
            }
        }

        public Visibility NumberVisibility
        {
            get=> ((Number??0) > 0 ? Visibility.Visible : Visibility.Collapsed); 
        }
        public string Cover
        {
            get
            {
                if (Source is IMHLBook book)
                    return book.Cover;
                return string.Empty;
            }
        }

        public IMHLBook? Book { get { return Source as IMHLBook; } }
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
            if (Source is IMHLBook book)
            {
                /* if (!string.IsNullOrEmpty(book.Annotation))
                     SourceItems.Add(new FB2Annotation(book.Annotation));*/

                if (book.Authors.Count > 0)
                    Add2Source(new FB2Authors(book, this));

                if (book.Genres.Count > 0)
                    Add2Source(new FB2Genres(book, this));

                if (book.Keywords.Count > 0)
                    Add2Source(new FB2Keywords(book, this));
            }
        }
        
        private void Add2Source(ITreeItem item)
        {
            if (Shower == null)
                SourceItems.Add(item);
            else
                Shower.Add2Source(item, this);
        }
        #endregion
    }
}

