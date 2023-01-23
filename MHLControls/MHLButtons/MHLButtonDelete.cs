using MHLCustomControls.Buttons;

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
