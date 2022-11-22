using System.Windows.Media;
using System.Windows;
using MHLCommon.MHLScanner;

namespace MHLSourceScannerLib
{
    public interface IDecorator4WPF : IDecorator
    {
        Brush ForeGround { get; }
        FontWeight FontWeight { get; }
    }
    public struct Decor4Unknown : IDecorator4WPF
    {
        public Brush ForeGround => Brushes.Black;
        public FontWeight FontWeight => FontWeights.Normal;
        public bool Focusable => true;
        public bool ThreeState => false;
    }

    public struct Decor4System : IDecorator4WPF
    {
        public Brush ForeGround => Brushes.Gray;
        public FontWeight FontWeight => FontWeights.Normal;
        public bool Focusable => false;
        public bool ThreeState => false;
    }

    public struct Decor4Error : IDecorator4WPF
    {
        public Brush ForeGround => Brushes.Red;
        public FontWeight FontWeight => FontWeights.Light;
        public bool Focusable => false;
        public bool ThreeState => false;
    }
}