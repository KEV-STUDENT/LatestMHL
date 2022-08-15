using System.Windows.Media;
using System.Windows;
namespace MHLSourceScannerLib
{
    public interface IDecorator
    {
        Brush ForeGround { get; }
        FontWeight FontWeight {get;}
        bool Focusable { get; }
    }

    public struct Decor4Unknown : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.Black;
        FontWeight IDecorator.FontWeight => FontWeights.Normal;
        bool IDecorator.Focusable => true;
    }

    public struct Decor4System : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.Gray;
        FontWeight IDecorator.FontWeight => FontWeights.Normal;
        bool IDecorator.Focusable => false;
    }

    public struct Decor4Error : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.Red;
        FontWeight IDecorator.FontWeight => FontWeights.Light;
        bool IDecorator.Focusable => false;
    }
}