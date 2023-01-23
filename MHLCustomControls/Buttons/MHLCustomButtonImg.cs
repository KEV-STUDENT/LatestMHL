using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MHLCustomControls.Buttons
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MHLCustomControls.Buttons"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MHLCustomControls.Buttons;assembly=MHLCustomControls.Buttons"
    ///
    /// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    /// на данный проект и пересобрать во избежание ошибок компиляции:
    ///
    ///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    ///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    ///
    ///
    /// Шаг 2)
    /// Теперь можно использовать элемент управления в файле XAML.
    ///
    ///     <MyNamespace:MHLCustomButtonImg/>
    ///
    /// </summary>
    public class MHLCustomButtonImg : MHLCustomButton, IButtonImgView
    {
        public static readonly DependencyProperty ImageSourceProperty;
        public static readonly DependencyProperty ImageWidthProperty;
        public static readonly DependencyProperty ImageHeightProperty;
        public static readonly DependencyProperty ImageMarginProperty;

        #region[Properties]
        public BitmapImage ImageSource {
            get { return (BitmapImage)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public Thickness ImageMargin
        {
            get { return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value);}
        }
        #endregion

        #region [Constructors]
        static MHLCustomButtonImg()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MHLCustomButtonImg), new FrameworkPropertyMetadata(typeof(MHLCustomButtonImg)));

            ImageSourceProperty = DependencyProperty.Register(
                "ImageSource",
                typeof(BitmapImage),
                typeof(MHLCustomButtonImg));

            ImageWidthProperty = DependencyProperty.Register(
               "ImageWidth",
               typeof(double),
               typeof(MHLCustomButtonImg));

            ImageHeightProperty = DependencyProperty.Register(
               "ImageHeight",
               typeof(double),
               typeof(MHLCustomButtonImg));

            ImageMarginProperty = DependencyProperty.Register(
                "ImageMargin",
                typeof(Thickness),
                typeof(MHLCustomButtonImg));
        }

        public MHLCustomButtonImg()
        {
            ViewModel = new VMButtonImg(this);
        }
        #endregion

        #region[IButtonImgView]
        IVMButtonImg IButtonImgView.ViewModel => (IVMButtonImg)ViewModel;
        BitmapImage IButtonImgView.ImageSource { get => ImageSource; set => ImageSource = value; }
        double IButtonImgView.ImageWidth { get => ImageWidth; set => ImageWidth = value; }
        double IButtonImgView.ImageHeight { get => ImageHeight; set => ImageHeight = value; }
        Thickness IButtonImgView.ImageMargin { get => ImageMargin;set => ImageMargin = value; }
        #endregion
    }
}
