using MHLResources;

namespace MHLControls.MHLButtons
{
    public class MHLButtonSettings : MHLButton
    {
        protected override void SetComponent()
        {
            base.SetComponent();
            Txt.Text = MHLResourcesManager.GetStringFromResources("MHLButtonSettings_CPT", "Settings");
            Img.Source = MHLResourcesManager.GetImageFromResources("SettingsButton");
        }
    }
}
