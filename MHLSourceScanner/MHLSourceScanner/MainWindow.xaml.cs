using MHLCommon;
using MHLCommon.MHLScanner;
using MHLControls.MHLPickers;
using MHLSourceScannerLib;
using System.Windows;

namespace MHLSourceScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel4Scanner _vm;
        public ViewModel4Scanner ViewModel {
            get { return _vm; }
        }
        public MainWindow()
        {
            _vm = new ViewModel4Scanner();
            _vm.CloseAction += () => { Close(); };
            _vm.ChangeDestinationDirAction += ChangeDestinationDir;
            _vm.ChangeSourceDirAction += ChangedSourceDir;
            _vm.SetDestinationDirAction += SettingsDestionnDir;

            InitializeComponent();           
            SourceDirectoryPicker.Caption = "Source Directory";
            SourceDirectoryPicker.CaptionWidth = 110;           
            SourceDirectoryPicker.AskUserForInputEvent += MHLAsk4Picker.AskDirectory;

            DestinationDirectoryPicker.AskUserForInputEvent += MHLAsk4Picker.AskDirectory;
            DestinationDBPicker.AskUserForInputEvent += MHLAsk4Picker.AskFile;

            DataContext = this;
        }

        private void SettingsDestionnDir()
        {
            DirectorySettings.DirSetting setting = new DirectorySettings.DirSetting();
            setting.ShowDialog();
        }

        private void ChangeDestinationDir()
        { 
        }


        private void ChangedSourceDir()
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
