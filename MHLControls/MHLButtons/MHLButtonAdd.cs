using MHLResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MHLControls.MHLButtons
{
    public class MHLButtonAdd : MHLButton
    {
        public MHLButtonAdd() : base()
        {
            //Caption = MHLResourcesManager.GetStringFromResources("MHLButtonAdd_CPT", "Add");
            Image = MHLResourcesManager.GetImageFromResources("Add_12x12");
            ImgHeight = 12;
            ImgWidth = 12;
            ButtonWidth = 16;
            ButtonHeight = 16;
            IsText = false;
        }
    }
}
