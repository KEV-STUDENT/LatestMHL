using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public class Decor4FB2AttrSection : Decorator4WPF
    {
        public override Brush ForeGround => Brushes.BlueViolet;
        public override FontWeight FontWeight => FontWeights.Bold;
    }

    public class FB2Authors : TreeViewFB2AttrSection<Decor4FB2AttrSection>
    {
        #region [Constructors]
        public FB2Authors(IBook book, ITreeItem? parent) : base(FB2Sections.Authors, book, parent)
        {
            Name = "Authors";
        }
        #endregion

        public override void LoadItemCollection()
        {
            foreach (MHLAuthor author in book.Authors)
            {
                SourceItems.Add(new FB2Author(author, this));
            }
        }
    }

    public class FB2Genres : TreeViewFB2AttrSection<Decor4FB2AttrSection>
    {
        #region [Constructors]
        public FB2Genres(IBook book, ITreeItem? parent) : base(FB2Sections.Genres, book, parent)
        {
            Name = "Genres";
        }
        #endregion

        public override void LoadItemCollection()
        {
            foreach (MHLGenre genre in book.Genres)
            {
                SourceItems.Add(new FB2Genre(genre, this));
            }
        }
    }

    public class FB2Keywords : TreeViewFB2AttrSection<Decor4FB2AttrSection>
    {
        #region [Constructors]
        public FB2Keywords(IBook book, ITreeItem? parent) : base(FB2Sections.Keywords, book, parent)
        {
            Name = "Keywords";
        }
        #endregion

        public override void LoadItemCollection()
        {
            foreach (MHLKeyword keyword in book.Keywords)
            {
                SourceItems.Add(new FB2Keyword(keyword, this));
            }
        }
    }


    public abstract class TreeViewFB2AttrSection<T> : TreeItemCollection where T : IDecorator4WPF, new()
    {
        #region [Fields]
        private readonly T decorator = new T();
        private readonly FB2Sections sectionType;
        protected readonly IBook book;
        #endregion

        #region [Proprties]
        public Brush ForeGround
        {
            get => decorator.ForeGround;
        }

        public FontWeight FontWeight
        {
            get => decorator.FontWeight;
        }

        public bool Focusable
        {
            get => decorator.Focusable;
        }
        #endregion

        #region [Constructors]
        public TreeViewFB2AttrSection(FB2Sections sectionType, IBook book, ITreeItem? parent):base(parent)
        {
            this.sectionType = sectionType;
            this.book = book;

            ITreeItemCollection treeCollection = this;
            treeCollection.LoadChilds();
        }
        #endregion

        #region [TreeItem implementation]
        public override void LoadChilds4Collection()
        {
            LoadItemCollection();
        }       
        #endregion
    }
}

