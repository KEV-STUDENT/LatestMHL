using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels.Buttons;
using MHLResources;

namespace ControlsCommon.ViewModels.Buttons
{
    public class MHLButtonRunVM : VMButtonImg
    {
        #region[Constructors]
        public MHLButtonRunVM(IButtonImgView buttonView) : base(buttonView) { }
        #endregion

        #region [Methods]
        protected override void SetCaption()
        {
            Caption = MHLResourcesManager.GetStringFromResources("MHLButtonRun_CPT", "Run");
        }

        protected override void SetImage()
        {
            ImageSource = MHLResourcesManager.GetImageFromResources("RunButton");
        }
        #endregion
    }
}