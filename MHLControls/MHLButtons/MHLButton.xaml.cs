using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MHLControls.MHLButtons
{
    /// <summary>
    /// Логика взаимодействия для MHLButton.xaml
    /// </summary>
    public partial class MHLButton : Button
    {

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(BitmapImage), typeof(MHLButton));

        #region [Fields]
        private BitmapImage? _image;
        private bool _useThisDataContext;
        #endregion

        #region [Properties]
        public BitmapImage? Image
        {
            get => _image;
            set => _image = value;
        }

        public int ImgWidth { get; set; }
        public int ImgHeight { get; set; }
        public string Caption { get; set; }

        public int ButtonHeight { get; set; }
        public int ButtonWidth { get; set; }

        public bool IsImage { get; set; }
        public bool IsText { get; set; }
        #endregion

        #region [Constructors]
        public MHLButton()
        {           
            InitializeComponent();
            SetComponent();
        }
        #endregion

        #region [Methods]
        protected virtual void SetComponent()
        {
            Img.Source = Image;
            Img.Width = 16;
            Img.Height = 16;

            Txt.Text = string.Empty;

            Height = 24;
            Width = 60;
        }
        #endregion
    }
}
