using MHLCommon;
using MHLCommon.MHLScanner;
using MHLControls.MHLPickers;
using MHLSourceScanner.Configurations.SourceFolder;
using MHLSourceScanner.Configurations.DestinationFolder;
using MHLSourceScannerLib;
using System;
using System.Configuration;
using System.Windows;
using System.Threading.Tasks;
using MHLSourceOnDisk;
using MHLControls.ViewModels4Forms;

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
            _vm = new ViewModel4Scanner();
            _vm.Close += () => {                 
                Close();
            };

            _vm.RunAsync += RunCommandAsync;           

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
            LoadDataFromConfig();
        }

        private async Task ExportSelectedDataAsync()
        {
            ExpOptions expOptions = new ExpOptions(DestinationDirectoryPicker.Value);
            Export2Dir exporter = new Export2Dir(expOptions);
            await SourceDirectoryTree.ViewModel.ExportSelectedItemsAsync(SourceDirectoryTree.SourceItems, exporter);
        }

        private void SaveConfigurations()
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SourceConfigSection section = (SourceConfigSection)cfg.Sections["SourceFolders"];
            if (section != null)
            {
                section.FolderItems[0].Path = SourceDirectoryPicker.Value;
            }

            DestinationConfigSection destinationSection = (DestinationConfigSection)cfg.Sections["DestinationFolders"];
            if (destinationSection != null)
            {
                if (_vm.DestinationIsDirectory)
                {
                    destinationSection.FolderItems[0].PathType = 1;
                    destinationSection.FolderItems[0].Path = DestinationDirectoryPicker.Value;
                }
                else
                {
                    destinationSection.FolderItems[0].PathType = 2;
                    destinationSection.FolderItems[0].Path = DestinationDBPicker.Value;
                }
            }
            cfg.Save();
        }

        private void LoadDataFromConfig()
        {
            SourceConfigSection section = (SourceConfigSection)ConfigurationManager.GetSection("SourceFolders");
            if (section != null && section.FolderItems.Count > 0)
            {
                SourceDirectoryPicker.Value = section.FolderItems[0].Path;
                _vm.ChangeSourceDirAction?.Invoke();
            }

            DestinationConfigSection destinationConfigSection = (DestinationConfigSection)ConfigurationManager.GetSection("DestinationFolders");
            if (destinationConfigSection != null && destinationConfigSection.FolderItems.Count > 0)
            {
                _vm.DestinationIsDirectory = (destinationConfigSection.FolderItems[0].PathType == 1);
                if (_vm.DestinationIsDirectory)
                    DestinationDirectoryPicker.Value = destinationConfigSection.FolderItems[0].Path;
                else
                    DestinationDBPicker.Value = destinationConfigSection.FolderItems[0].Path;
            }
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

        private async Task RunCommandAsync()
        {
            _vm.NotBusy = false;
            SaveConfigurations();
            await ExportSelectedDataAsync();
            _vm.NotBusy = true;
        }

        /*private void ExportSelectedData(VMEditForm vm)
       {
           ExpOptions expOptions = new ExpOptions(DestinationDirectoryPicker.Value);
           Export2Dir exporter = new Export2Dir(expOptions);
           SourceDirectoryTree.ViewModel.ExportSelectedItems(SourceDirectoryTree.SourceItems, exporter);
       }
        private void RunCommand()
        {
            _vm.NotBusy = false;
            SaveConfigurations();
            ExportSelectedData(_vm);
        }

        private void SetBusy(Task task)
        {
            _vm.NotBusy = true;
        }*/
    }
}
