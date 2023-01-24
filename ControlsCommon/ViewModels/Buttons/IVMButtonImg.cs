using System.Windows;
using System.Windows.Media.Imaging;

namespace ControlsCommon.ViewModels.Buttons
{
    public interface IVMButtonImg : IVMButton
    {
        BitmapImage? ImageSource { get; set; }
        double ImageWidth { get; set; }
        double ImageHeight { get; set; }
        Thickness ImageMargin { get; set; }
    }
}
