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

    public class Decorator4WPF : IDecorator4WPF
    {
        Brush IDecorator4WPF.ForeGround => this.ForeGround;
        FontWeight IDecorator4WPF.FontWeight => this.FontWeight;

        bool IDecorator.Focusable => this.Focusable;

        bool IDecorator.ThreeState => this.ThreeState;

        public virtual Brush ForeGround => Brushes.Black;
        public virtual FontWeight FontWeight => FontWeights.Normal;
        public virtual bool Focusable => false;
        public virtual bool ThreeState => false;
    }
    public class Decor4Unknown : Decorator4WPF
    {
        public override bool Focusable => true;
    }

    public class Decor4System : Decorator4WPF
    {
        public override Brush ForeGround => Brushes.Gray;
    }

    public class Decor4Error : Decorator4WPF
    {
        public override Brush ForeGround => Brushes.Red;
        public override FontWeight FontWeight => FontWeights.Light;
    }
}