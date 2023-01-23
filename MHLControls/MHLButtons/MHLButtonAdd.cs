using MHLCustomControls.Buttons;

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
