using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MHLControls.MHLButtons
{
    /// <summary>
    /// Логика взаимодействия для MHLButton.xaml
    /// </summary>
    public partial class MHLButton : Button
    {
        private BitmapImage? _image;

        public BitmapImage? Image
        {
            get => _image;
            set =>_image = value;
        }

        public int ImgWidth { get; set; }
        public int ImgHeigh { get; set; }   
        public string Caption { get; set; }
        public MHLButton()
        {
            ImgHeigh = 16;
            ImgWidth= 16;
            Caption= string.Empty;
            InitializeComponent();
            DataContext = this;
        }
    }
}
