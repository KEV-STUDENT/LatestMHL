using MHLCommands;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLCommon;
using MHLControls.ViewModels4Forms;
using MHLSourceScannerLib;
using System;
using System.Windows.Input;
using System.Threading.Tasks;
using MHLSourceScanner.Configurations.DestinationFolder;
using MHLSourceScanner.Configurations.SourceFolder;
using System.Configuration;

namespace MHLSourceScanner
{
    public class ViewModel4Scanner : VMEditForm
    {
        #region [Fields]
        private bool _destinationIsDirectory = true;
        private string _destinationDB = string.Empty;
        private string _destinationPath = string.Empty;
        private string _sourcePath = string.Empty;
        private Model4Scanner _model;
        MainWindow _view;
        #endregion

        #region [Constructors]
        public ViewModel4Scanner(MainWindow mainWindow) :base()
        {
            _model = new Model4Scanner();
            _view = mainWindow;
            ChangeSourceDirCommand = new RelayCommand(ExecuteChangeSourceDirCommand, CanExecuteChangeSourceDirCommand);
            ChangeSourceDirAction = ChangeSourceDir;

            SetExportDirCommand = new RelayCommand(ExecuteSetExportDirCommand, CanExecuteSetExportDirCommand);
            SetExportDirAction = SetExportDir;

            RunAsync += SaveSettinngsAndExportData;
        }
        #endregion

        #region [Properties]
        public Action? ChangeSourceDirAction;
        public ICommand ChangeSourceDirCommand { get; set; }

        public Action? SetExportDirAction;
        public ICommand SetExportDirCommand { get; set; }

        public bool DestinationIsDirectory
        {
            get { return _destinationIsDirectory; }
            set
            {
                _destinationIsDirectory = value;
                OnPropertyChanged("DestinationIsDBFile");
                OnPropertyChanged("DestinationIsDirectory");
            }
        }

        public bool DestinationIsDBFile
        {
            get { return !_destinationIsDirectory; }
            set
            {
                _destinationIsDirectory = !value;
                OnPropertyChanged("DestinationIsDBFile");
                OnPropertyChanged("DestinationIsDirectory");
            }
        }

        public string DestinationDB
        {
            get
            {
                return _destinationDB;
            }

            set
            {
                _destinationDB = value;
                OnPropertyChanged("DestinationDB");
            }
        }

        public string DestinationPath
        {
            get
            {
                return _destinationPath;
            }

            set
            {
                _destinationPath = value;
                OnPropertyChanged("DestinationPath");
            }
        }

        public string SourcePath
        {
            get
            {
                return _sourcePath;
            }

            set
            {
                _sourcePath = value;
                OnPropertyChanged("SourcePath");
            }
        }
        #endregion

        #region [Methods]
        private void ExecuteChangeSourceDirCommand(object? obj)
        { ChangeSourceDirAction?.Invoke(); }

        private bool CanExecuteChangeSourceDirCommand(object? obj)
        { return ChangeSourceDirAction != null; }


        private bool CanExecuteSetExportDirCommand(object? obj)
        {
            return SetExportDirAction != null;
        }

        private void ExecuteSetExportDirCommand(object? obj)
        {
           SetExportDirAction?.Invoke();
        }

        private void ChangeSourceDir()
        {
            if(_view.SourceDirectoryTree != null)
                _model.ChangeSourceDir(SourcePath, _view.SourceDirectoryTree);
        }

        private void SetExportDir()
        {
            DirectorySettings.DirSetting setting = new DirectorySettings.DirSetting();
            setting.ShowDialog();
        }

        private async Task SaveSettinngsAndExportData(object? obj)
        {
            NotBusy = false;
            if(_destinationIsDirectory)
                _model.SaveConfigurations(SourcePath, 1, DestinationPath);
            else
                _model.SaveConfigurations(SourcePath, 2, DestinationDB);

            await _model.ExportSelectedDataAsync(_view.SourceDirectoryTree.SourceItems, DestinationPath).ContinueWith((t) => { NotBusy = t.IsCompleted; });
        }

        internal void LoadDataFromConfig()
        {
            SourceConfigSection section = (SourceConfigSection)ConfigurationManager.GetSection("SourceFolders");
            if (section != null && section.FolderItems.Count > 0)
                SourcePath = section.FolderItems[0].Path;

            DestinationConfigSection destinationConfigSection = (DestinationConfigSection)ConfigurationManager.GetSection("DestinationFolders");
            if (destinationConfigSection != null && destinationConfigSection.FolderItems.Count > 0)
            {
                DestinationIsDirectory = (destinationConfigSection.FolderItems[0].PathType == 1);
                if (DestinationIsDirectory)
                    DestinationPath = destinationConfigSection.FolderItems[0].Path;
                else
                    DestinationDB = destinationConfigSection.FolderItems[0].Path;
            }
        }
        #endregion
    }
}
