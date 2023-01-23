using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels;
using MHLResources;
using System.Windows;

namespace MHLControls.MHLButtons
{
    internal class MHLButtonAddVM : VMButtonImg
    {
        #region [Constructors]
        public MHLButtonAddVM(IButtonImgView buttonView) : base(buttonView) { }
        #endregion

        #region [Methods]
        protected override void SetCaption()
        {
            Caption = string.Empty;
        }
        protected override void SetSize()
        {
            Width = 14;
            Height = 14;
        }
        protected override void SetImage()
        {
            ImageSource = MHLResourcesManager.GetImageFromResources("Add_12x12");
        }
        protected override void SetImageSize()
        {
            ImageWidth = 10;
            ImageHeight = 10;
            ImageMargin = new Thickness(2);
        }
        #endregion
    }
}