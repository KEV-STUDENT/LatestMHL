using ControlsCommon.ControlsViews;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ControlsCommon.ViewModels
{
    public class VMButtonImg : VMButton, IVMButtonImg
    {
        #region [Constructors]
        public VMButtonImg(IButtonImgView buttonView) : base(buttonView) { }
        #endregion

        #region[Properties]
        public BitmapImage? ImageSource
        {
            get
            {
                return (((IVMButton)this).ButtonView as IButtonImgView)?.ImageSource;
            }
            set
            {
                if (value != null && (((IVMButton)this).ButtonView is IButtonImgView view))
                {
                    view.ImageSource = value;
                }
            }
        }

        public double ImageWidth
        {
            get
            {
                return (((IVMButton)this).ButtonView as IButtonImgView)?.ImageWidth ?? 0;
            }
            set
            {
                if (((IVMButton)this).ButtonView is IButtonImgView view)
                {
                    view.ImageWidth = value;
                }
            }
        }

        public double ImageHeight
        {
            get
            {
                return (((IVMButton)this).ButtonView as IButtonImgView)?.ImageHeight ?? 0;
            }
            set
            {
                if (((IVMButton)this).ButtonView is IButtonImgView view)
                {
                    view.ImageHeight = value;
                }
            }
        }

        public Thickness ImageMargin {
            get
            {
                return (((IVMButton)this).ButtonView as IButtonImgView)?.ImageMargin ?? new Thickness(0);
            }
            set
            {
                if (((IVMButton)this).ButtonView is IButtonImgView view)
                {
                    view.ImageMargin = value;
                }
            }
        }
        #endregion

        #region [Methods]
        protected override void SetProperties()
        {
            base.SetProperties();
            SetImage();
            SetImageSize();
        }

        protected virtual void SetImage()
        {
        }
        protected virtual void SetImageSize()
        {
            ImageWidth = 16;
            ImageHeight= 16;
            ImageMargin = new Thickness(4, 0, 0, 0);
        }
        #endregion

        #region[IVMButtonImg]
        BitmapImage? IVMButtonImg.ImageSource { get => ImageSource; set => ImageSource = value; }
        double IVMButtonImg.ImageWidth { get => ImageWidth; set => ImageWidth = value; }
        double IVMButtonImg.ImageHeight { get => ImageHeight; set => ImageHeight = value; }
        Thickness IVMButtonImg.ImageMargin { get=> ImageMargin; set=> ImageMargin=value; }        
        #endregion
    }
}