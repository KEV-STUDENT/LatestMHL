using System.Windows;
using System.Windows.Media.Imaging;
using ControlsCommon.ViewModels.Buttons;

namespace ControlsCommon.ControlsViews
{
    public interface IButtonImgView : IButtonView
    {
        new IVMButtonImg ViewModel { get; }
        BitmapImage ImageSource { get; set; }
        double ImageWidth { get; set; }
        double ImageHeight { get; set; }
        Thickness ImageMargin { get; set; }
    }
}