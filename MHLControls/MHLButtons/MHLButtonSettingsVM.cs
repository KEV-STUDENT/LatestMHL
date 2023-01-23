using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels;
using MHLResources;

namespace MHLControls.MHLButtons
{
    internal class MHLButtonSettingsVM : VMButtonImg
    {
        #region[Constructors]
        public MHLButtonSettingsVM(IButtonImgView buttonView) : base(buttonView) { }
        #endregion

        #region [Methods]
        protected override void SetCaption()
        {
            Caption = MHLResourcesManager.GetStringFromResources("MHLButtonSettings_CPT", "Settings");
        }

        protected override void SetImage()
        {
            ImageSource = MHLResourcesManager.GetImageFromResources("SettingsButton");
        }
        #endregion
    }
}