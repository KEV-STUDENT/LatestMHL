using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MHLControls.MHLButtons
{
    /// <summary>
    /// Логика взаимодействия для MHLButton.xaml
    /// </summary>
    public partial class MHLButton : Button
    {
        #region [Fields]
        private BitmapImage? _image;
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
            ImgHeight = 16;
            ImgWidth = 16;

            ButtonHeight = 24;
            ButtonWidth = 60;

            IsImage = true;
            IsText = true;

            Caption = string.Empty;
            InitializeComponent();
            DataContext = this;
        }
        #endregion
    }
}
