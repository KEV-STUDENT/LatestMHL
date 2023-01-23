using ControlsCommon.ControlsViews;
using MHLCommon.ViewModels;

namespace ControlsCommon.ViewModels
{
    public class VMButton : ViewModel, IVMButton
    {
        private IButtonView buttonView;

        #region [Constructors]
        public VMButton(IButtonView buttonView)
        {
            this.buttonView = buttonView;
            SetProperties();
        }

        protected virtual void SetProperties()
        {
            SetFont();
            SetCaption();
            SetSize();
        }
        #endregion

        #region[Methods]
        protected virtual void SetSize() { }
        protected virtual void SetCaption() { }
        protected virtual void SetFont() { }
        #endregion

        #region [Properties]
        public string Caption { get => buttonView.Caption; set => buttonView.Caption = value; }
        public double Width { get => buttonView.Width; set { buttonView.Width = value; } }
        public double Height { get => buttonView.Height; set { buttonView.Height = value; } }

        System.Windows.Media.FontFamily FontName { get => buttonView.FontName; set => buttonView.FontName = value; }
        double FontSize { get => buttonView.FontSize; set => buttonView.FontSize = value; }
        #endregion

        #region[IVMButton]
        IButtonView IVMButton.ButtonView => buttonView;
        string IVMButton.Caption { get => Caption; set => Caption = value; }
        double IVMButton.Width { get => Width; set => Width = value; }
        double IVMButton.Height { get => Height; set => Height = value; }
        System.Windows.Media.FontFamily IVMButton.FontName { get => FontName; set => FontName = value; }
        double IVMButton.FontSize { get => FontSize; set => FontSize = value; }
        #endregion
    }
}