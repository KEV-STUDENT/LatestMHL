using MHLCustomControls.Buttons;

namespace MHLControls.MHLButtons
{
    public class MHLButtonRun : MHLCustomButtonImg
    {
        public MHLButtonRun()
        {
            ViewModel = new MHLButtonRunVM(this);
        }       
    }
}