using MHLResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLControls.MHLButtons
{
    public class MHLButtonRun : MHLButton
    {
        public MHLButtonRun() : base()
        {
            Caption = MHLResourcesManager.GetStringFromResources("MHLButtonRun_CPT", "Run");
            Image = MHLResourcesManager.GetImageFromResources("RunButton");
        }
    }
}