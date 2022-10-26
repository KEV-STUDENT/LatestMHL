using MHLResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLControls.MHLButtons
{
    public class MHLButtonClose : MHLButton
    {
        public MHLButtonClose() : base()
        {
            Caption = MHLResourcesManager.GetStringFromResources("MHLButtonClose_CPT", "Close");
            Image = MHLResourcesManager.GetImageFromResources("CloseButton");
        }
    }
}
