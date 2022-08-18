using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4FB2AttrSection : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.BlueViolet;
        FontWeight IDecorator.FontWeight => FontWeights.Bold;
        bool IDecorator.Focusable => false;
    }

    public class FB2Authors : TreeViewFB2AttrSection<Decor4FB2AttrSection>
    {
        #region [Constructors]
        public FB2Authors(IBook book) : base(FB2Sections.Authors, book)
        {
            Name = "Authors";
        }
        #endregion

        public override void LoadItemCollection()
        {
            foreach (MHLAuthor author in book.Authors)
            {
                SourceItems.Add(new FB2Author(author));
            }
        }
    }

    public class FB2Genres : TreeViewFB2AttrSection<Decor4FB2AttrSection>
    {
        #region [Constructors]
        public FB2Genres(IBook book) : base(FB2Sections.Genres, book)
        {
            Name = "Genres";
        }
        #endregion

        public override void LoadItemCollection()
        {
            foreach (MHLGenre genre in book.Genres)
            {
                SourceItems.Add(new FB2Genre(genre));
            }
        }
    }

    public class FB2Keywords : TreeViewFB2AttrSection<Decor4FB2AttrSection>
    {
        #region [Constructors]
        public FB2Keywords(IBook book) : base(FB2Sections.Keywords, book)
        {
            Name = "Keywords";
        }
        #endregion

        public override void LoadItemCollection()
        {
            foreach (MHLKeyword keyword in book.Keywords)
            {
                SourceItems.Add(new FB2Keyword(keyword));
            }
        }
    }


    public abstract class TreeViewFB2AttrSection<T> : TreeCollectionItem where T : IDecorator, new()
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
        public TreeViewFB2AttrSection(FB2Sections sectionType, IBook book)
        {
            this.sectionType = sectionType;
            this.book = book;

            ITreeCollectionItem treeCollection = this;
            treeCollection.LoadChilds();
        }
        #endregion

        #region [TreeItem implementation]
        public override void LoadChilds()
        {
            LoadItemCollection();
        }       
        #endregion
    }
}

