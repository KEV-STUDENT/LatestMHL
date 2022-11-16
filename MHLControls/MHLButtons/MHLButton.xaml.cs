using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MHLResources;

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
