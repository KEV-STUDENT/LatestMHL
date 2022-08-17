using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4FB2Attr : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.Black;
        FontWeight IDecorator.FontWeight => FontWeights.Normal;
        bool IDecorator.Focusable => false;
    }

    public class FB2Author : TreeViewFB2Attr<Decor4FB2Attr, MHLAuthor>
    {
        #region [Proprties]
        public string LastName
        {
            get => bookAttribute.LastName;
        }

        public string FirstName
        {
            get => bookAttribute.FirstName;
        }

        public string MiddleName
        {
            get => bookAttribute.MiddleName;
        }
        #endregion

        #region [Constructors]
        public FB2Author(MHLAuthor bookAttribute) : base(bookAttribute)
        {
        }
        #endregion
    }


    public abstract class TreeViewFB2Attr<T1, T2> : TreeItem
    where T1 : IDecorator, new()
    where T2 : IBookAttribute
    {
        #region [Fields]
        private readonly T1 decorator = new T1();
        protected readonly T2 bookAttribute;
        #endregion

        #region [Constructors]
        public TreeViewFB2Attr(T2 bookAttribute)
        {
            this.bookAttribute = bookAttribute;
        }
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
    }
}
