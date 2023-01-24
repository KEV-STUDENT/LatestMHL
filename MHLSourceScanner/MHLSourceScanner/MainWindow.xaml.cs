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
            SourceDirectoryPicker.AskUserForInputEvent += MHLAsk4Picker.AskDirectory;

            DestinationDirectoryPicker.AskUserForInputEvent += MHLAsk4Picker.AskDirectory;
            DestinationDirectoryPicker.AskUserSettings += _vm.SetExportDirAction;

            DestinationDBPicker.AskUserForInputEvent += MHLAsk4Picker.AskFile;

            TestPicker.AskUserForInputEvent += MHLAsk4Picker.AskDirectory;

            DataContext = this;
            _vm.LoadDataFromConfig();
        }
    }
}
