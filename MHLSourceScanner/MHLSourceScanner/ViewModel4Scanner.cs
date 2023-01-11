using MHLCommands;
using MHLCommon.ViewModels;
using MHLControls.ViewModels4Forms;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MHLSourceScanner
{
    public class ViewModel4Scanner : VMEditForm, IVM4Scanner
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

        #region [IVM4Scanner Implementation]
        bool IVM4Scanner.DestinationIsDirectory { get => DestinationIsDirectory; set => DestinationIsDirectory = value; }
        bool IVM4Scanner.DestinationIsDBFile { get => DestinationIsDBFile; set => DestinationIsDBFile = value; }
        string IVM4Scanner.DestinationDB { get => DestinationDB; set => DestinationDB = value; }
        string IVM4Scanner.DestinationPath { get => DestinationPath; set => DestinationPath = value; }
        string IVM4Scanner.SourcePath { get => SourcePath; set => SourcePath = value; }
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
            _model.LoadConfigurations((IVM4Scanner)this);
        }
        #endregion
    }
}
