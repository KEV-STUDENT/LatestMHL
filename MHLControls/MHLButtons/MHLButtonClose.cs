using MHLCustomControls.Buttons;
using ControlsCommon.ViewModels.Buttons;

namespace MHLControls.MHLButtons
{
    public class MHLButtonClose : MHLCustomButtonImg
    {
        public MHLButtonClose() {
            ViewModel = new MHLButtonCloseVM(this);
        }
    }
}
