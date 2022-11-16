using MHLResources;

namespace MHLControls.MHLButtons
{
    public class MHLButtonSettings : MHLButton
        {
            public MHLButtonSettings() : base()
            {
                Caption = MHLResourcesManager.GetStringFromResources("MHLButtonSettings_CPT", "Settings");
                Image = MHLResourcesManager.GetImageFromResources("SettingsButton");
            }
        }
    }
