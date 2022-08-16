using MHLCommon;
using MHLSourceScannerModelLib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4FB2AttrSection : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.BlueViolet;
        FontWeight IDecorator.FontWeight => FontWeights.Bold;
        bool IDecorator.Focusable => true;
    }

    public class TreeViewFB2Section : TreeViewFB2AttrSection<Decor4FB2AttrSection>
    { 
    }

     public class TreeViewFB2AttrSection<T> : TreeAttributeCollectionItem where T : IDecorator, new()
    {
        private readonly T decorator = new T();
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

        #region [Constructors]
        #endregion

        #region [Protected Methods]
        #endregion
    }
}

