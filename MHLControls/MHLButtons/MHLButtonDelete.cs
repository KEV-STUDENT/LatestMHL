using MHLCustomControls.Buttons;
using ControlsCommon.ViewModels.Buttons;

namespace MHLControls.MHLButtons
{
    public class MHLButtonDelete : MHLCustomButtonImg
    {
        public MHLButtonDelete()
        {
            ViewModel = new MHLButtonDeleteVM(this);
        }
    }
}
