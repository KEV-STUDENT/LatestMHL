using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels;
using MHLResources;

namespace MHLControls.MHLButtons
{
    internal class MHLButtonCloseVM : VMButtonImg
    {
        #region [Constructors]
        public MHLButtonCloseVM(IButtonImgView buttonView) : base(buttonView) { }
        #endregion

        #region [Methods]
        protected override void SetCaption()
        {
            Caption = MHLResourcesManager.GetStringFromResources("MHLButtonClose_CPT", "Close");
        }

        protected override void SetImage()
        {
            ImageSource = MHLResourcesManager.GetImageFromResources("CloseButton");
        }
        #endregion
    }
}