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
        protected override void SetComponent()
        {
            base.SetComponent();
            Txt.Text = MHLResourcesManager.GetStringFromResources("MHLButtonRun_CPT", "Run");
            Img.Source = MHLResourcesManager.GetImageFromResources("RunButton");
        }
    }
}