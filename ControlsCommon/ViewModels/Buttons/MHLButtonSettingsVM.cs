using ControlsCommon.ControlsViews;
using MHLResources;

namespace ControlsCommon.ViewModels.Buttons
{
    public class MHLButtonSettingsVM : VMButtonImg
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