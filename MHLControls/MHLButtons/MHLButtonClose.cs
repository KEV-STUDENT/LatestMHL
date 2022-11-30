using MHLResources;

namespace MHLControls.MHLButtons
{
    public class MHLButtonClose : MHLButton
    {
        protected override void SetComponent()
        {
            base.SetComponent();
            Txt.Text = MHLResourcesManager.GetStringFromResources("MHLButtonClose_CPT", "Close");
            Img.Source = MHLResourcesManager.GetImageFromResources("CloseButton");
        }
    }
}
