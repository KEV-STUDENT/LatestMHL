using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels;
using MHLResources;

namespace MHLControls.MHLButtons
{
    internal class MHLButtonRunVM : VMButtonImg
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