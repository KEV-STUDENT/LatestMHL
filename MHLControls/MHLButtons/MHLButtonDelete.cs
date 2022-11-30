using MHLResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLControls.MHLButtons
{
    public class MHLButtonDelete : MHLButton
    {
        public MHLButtonDelete() : base()
        {
            //Caption = MHLResourcesManager.GetStringFromResources("MHLButtonDelete_CPT", "Delete");
            Image = MHLResourcesManager.GetImageFromResources("Minus_12x12");
            ImgHeight = 12;
            ImgWidth = 12;
            ButtonWidth = 16;
            ButtonHeight = 16;
            IsText = false;
        }
    }
}
