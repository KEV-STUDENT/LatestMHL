using MHLCommon;
using MHLCommon.MHLScanner;
using MHLControls.MHLPickers;
using MHLSourceScannerLib;
using MHLUIElements;
using System.ComponentModel;
using System.Windows;
using MHLResources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


namespace MHLSourceScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel4Scanner viewModel;
        public ViewModel4Scanner ViewModel {
            get { return viewModel; }
        }
        public MainWindow()
        {
            viewModel = new ViewModel4Scanner();
            InitializeComponent();           
            SourceDirectoryPicker.Caption = "Source Directory";
            SourceDirectoryPicker.CaptionWidth = 110;
            SourceDirectoryPicker.PropertyChanged += SourceDirectoryPickerChanged;
            SourceDirectoryPicker.AskUserForInput += MHLAsk4Picker.AskDirectory;

            DestinationDirectoryPicker.AskUserForInput += MHLAsk4Picker.AskDirectory;
            DestinationDBPicker.AskUserForInput += MHLAsk4Picker.AskFile;

            DataContext = this;
            viewModel.CloseAction += () => { Close(); };
        }


        private void SourceDirectoryPickerChanged(object? sender, PropertyChangedEventArgs e)
        {
            string directory;

            if (((IPicker<string>)SourceDirectoryPicker).CheckValue(out directory) == ReturnResultEnum.Ok)
            {
                IShower shower = SourceDirectoryTree;
                ITreeDiskItem treeViewDiskItem = new TreeViewDirectory(directory, shower, null);
                shower.SourceItems.Clear();
                shower.UpdateView(treeViewDiskItem);
            }
        }
    }
}
