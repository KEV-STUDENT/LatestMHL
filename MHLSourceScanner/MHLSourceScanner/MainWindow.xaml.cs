using MHLControls.MHLPickers;
using System.Windows;

namespace MHLSourceScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel4Scanner _vm;
        public ViewModel4Scanner ViewModel
        {
            get { return _vm; }
        }
        public MainWindow()
        {
            _vm = new ViewModel4Scanner(this);
            _vm.Close += () => {                 
                Close();
            };

            InitializeComponent();

            SourceDirectoryPicker.Caption = "Source Directory";
            SourceDirectoryPicker.CaptionWidth = 110;
            SourceDirectoryPicker.AskUserForInputEvent += MHLAsk4Picker.AskDirectory;

            DestinationDirectoryPicker.AskUserForInputEvent += MHLAsk4Picker.AskDirectory;
            DestinationDBPicker.AskUserForInputEvent += MHLAsk4Picker.AskFile;

            DataContext = this;
            _vm.LoadDataFromConfig();
        }
    }
}
