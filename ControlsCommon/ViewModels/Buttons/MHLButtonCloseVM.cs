using ControlsCommon.ControlsViews;
using MHLResources;

namespace ControlsCommon.ViewModels.Buttons
{
    public class MHLButtonCloseVM : VMButtonImg
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