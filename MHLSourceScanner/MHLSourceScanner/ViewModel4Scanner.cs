using MHLCommands;
using MHLCommon;
using MHLCommon.ViewModels;
using MHLControls.ViewModels4Forms;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Reflection.Metadata;

namespace MHLSourceScanner
{
    public class ViewModel4Scanner : VMEditForm, IVM4Scanner
    {
        #region [Fields]       
        private string _destinationDB = string.Empty;
        private string _destinationPath = string.Empty;
        private string _destinationMSSqlDB = "Test";//string.Empty;
        private string _sourcePath = string.Empty;
        private Model4Scanner _model;
        MainWindow _view;
        #endregion

        #region [Constructors]
        public ViewModel4Scanner(MainWindow mainWindow) : base()
        {
            _model = new Model4Scanner();
            _view = mainWindow;
            ChangeSourceDirCommand = new RelayCommand(ExecuteChangeSourceDirCommand, CanExecuteChangeSourceDirCommand);
            ChangeSourceDirAction = ChangeSourceDir;

            SetExportDirCommand = new RelayCommand(ExecuteSetExportDirCommand, CanExecuteSetExportDirCommand);
            SetExportDirAction = SetExportDir;

            RunAsync += SaveSettinngsAndExportData;

            TypeChangedCommand = new RelayCommand(ExecuteTypeChangedCommand, CanExecuteTypeChangedCommand);
        }
        #endregion



        #region [Properties]
        public ICommand TypeChangedCommand { get; set; }

        public Action? ChangeSourceDirAction;
        public ICommand ChangeSourceDirCommand { get; set; }

        public Action? SetExportDirAction;
        public ICommand SetExportDirCommand { get; set; }

        public bool DestinationIsDirectory
        {
            get { return ExportType == ExportEnum.Directory; }
            set
            {
                if(value)
                    ExportType = ExportEnum.Directory;
            }
        }

        public bool DestinationIsDBFile
        {
            get { return ExportType == ExportEnum.SQLite; }
            set
            {
                if(value)
                    ExportType = ExportEnum.SQLite;
            }
        }

        public bool DestinationIsMSSqlDB
        {
            get { return ExportType == ExportEnum.MSSQLServer; }
            set
            {
                if (value)
                    ExportType = ExportEnum.MSSQLServer;
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

        public string DestinationMSSqlDB
        {
            get
            {
                return _destinationMSSqlDB;
            }

            set
            {
                _destinationMSSqlDB = value;
                OnPropertyChanged("DestinationMSSqlDB");
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

        public ExportEnum ExportType
        {
            get => _view.DestinationType.ViewModel.SelectedItem.ExportType;
            set {
                 _view.DestinationType.ViewModel.SelectedItem =(
                    from s in _view.DestinationType.ViewModel.Source
                        where s.ExportType == value
                        select s).FirstOrDefault();

                OnPropertyChanged("DestinationIsDBFile");
                OnPropertyChanged("DestinationIsDirectory");
                OnPropertyChanged("DestinationIsMSSqlDB");
            }
        }
        #endregion

        #region [IVM4Scanner Implementation]
        ExportEnum IVM4Scanner.ExportType { get => ExportType; set => ExportType = value; }
        string IVM4Scanner.DestinationDB { get => DestinationDB; set => DestinationDB = value; }
        string IVM4Scanner.DestinationPath{get => DestinationPath;set => DestinationPath = value;}
        string IVM4Scanner.DestinationMSSqlDB { get => DestinationMSSqlDB; set => DestinationMSSqlDB = value; }
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
            if (_view.SourceDirectoryTree != null)
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
            _model.SaveConfigurations(SourcePath, ExportType, DestinationPath, DestinationDB);

            switch (ExportType)
            {
                case (ExportEnum.Directory):
                    await _model.ExportSelectedData2DirAsync(_view.SourceDirectoryTree.SourceItems, DestinationPath).ContinueWith((t) => { NotBusy = t.IsCompleted; });
                    break;
                case (ExportEnum.SQLite):
                    await _model.ExportSelectedData2SQLiteAsync(_view.SourceDirectoryTree.SourceItems, DestinationDB).ContinueWith((t) => { NotBusy = t.IsCompleted; });
                    break;
                default:
                    NotBusy = true;
                    break;
            }
        }

        internal void LoadDataFromConfig()
        {
            _model.LoadConfigurations((IVM4Scanner)this);
        }

        private void ExecuteTypeChangedCommand(object? obj)
        {
            OnPropertyChanged("DestinationIsDBFile");
            OnPropertyChanged("DestinationIsDirectory");
            OnPropertyChanged("DestinationIsMSSqlDB");
        }
        private bool CanExecuteTypeChangedCommand(object? obj)
        {
            return true;
        }
        #endregion
    }
}
