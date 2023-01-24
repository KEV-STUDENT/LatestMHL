using MHLCustomControls.Buttons;
using ControlsCommon.ViewModels.Buttons;

namespace MHLControls.MHLButtons
{
    public class MHLButtonSettings : MHLCustomButtonImg
    {

        public MHLButtonSettings()
        {
            ViewModel = new MHLButtonSettingsVM(this);
        }       
    }
}
