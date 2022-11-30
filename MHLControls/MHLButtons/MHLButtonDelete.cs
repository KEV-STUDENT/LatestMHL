using MHLResources;
using System.Windows;

namespace MHLControls.MHLButtons
{
    public class MHLButtonDelete : MHLButton
    {
        protected override void SetComponent()
        {
            Img.Source = MHLResourcesManager.GetImageFromResources("Minus_12x12");
            Img.Width = 12;
            Img.Height = 12;

            Txt.Text = string.Empty;
            Txt.Visibility = Visibility.Collapsed;

            Height = 16;
            Width = 16;
        }
    }
}
