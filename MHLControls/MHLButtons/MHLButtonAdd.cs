using MHLCustomControls.Buttons;
using ControlsCommon.ViewModels.Buttons;

namespace MHLControls.MHLButtons
{
    public class MHLButtonAdd : MHLCustomButtonImg
    {
        public MHLButtonAdd()
        {
            ViewModel = new MHLButtonAddVM(this);
        }        
    }
}
