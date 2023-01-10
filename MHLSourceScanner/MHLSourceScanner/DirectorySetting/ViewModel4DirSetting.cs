using MHLCommands;
using MHLControls.ViewModels4Forms;
using MHLSourceScanner.Configurations.RowFolder;
using MHLSourceScanner.DirectorySetting;
using MHLSourceScannerLib.BookDir;
using System;
using System.Configuration;
using System.Text.Json;
using System.Windows.Input;

namespace MHLSourceScanner.DirectorySettings
{
    public class ViewModel4DirSetting : VMEditForm
    {
        #region [Fields]
        private Model4DirSetting _model;
        private DirSetting _view;
        #endregion

        #region [Properies]
        public ICommand AddRowCommand { get; set; }
        public ICommand DeleteRowCommand { get; set; }
        #endregion

        public ViewModel4DirSetting(DirSetting dirSetting) : base()
        {
            AddRowCommand = new RelayCommand(ExecuteAddRowCommand, CanExecuteAddRowCommand);
            DeleteRowCommand = new RelayCommand(ExecuteDeleteRowCommand, CanExecuteDeleteRowCommand);
            _model = new Model4DirSetting();
            _view = dirSetting;

            Close += () => {
                _view.Close();
            };

            Run += () =>
            {
                SaveData2Json();
                _view.Close();
            };
        }

        private void SaveData2Json()
        {
           /* PathRowVM? row = _view.DirectoryTree.ViewModel.Source[0];

            string jsonString = JsonSerializer.Serialize<PathRowVM?>(row);


            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            RowConfigSection section = (RowConfigSection)cfg.Sections["RowStructure"];
            if (section != null)
            {
                section.RowItems[0].StructureJson = jsonString;
                cfg.Save();
            }*/
        }

        #region [Methods]
        private void ExecuteAddRowCommand(object? obj)
        {
            if(obj is DirSetting form) {
                _model.AddRow2Parent(form.DirectoryTree.SelectedItem);                   
            }
        }
        private bool CanExecuteAddRowCommand(object? obj)
        {
            return (obj is DirSetting form) && _model.CanAddRow2Parent(form.DirectoryTree.SelectedItem);                
        }

        private void ExecuteDeleteRowCommand(object? obj)
        {
            if (obj is DirSetting form)
            {
                _model.DeleteRowFromParent(form.DirectoryTree.SelectedItem);                
            }
        }
        private bool CanExecuteDeleteRowCommand(object? obj)
        {
            return (obj is DirSetting form) && _model.CanDeleteFromParent(form.DirectoryTree.SelectedItem);               
        }
        #endregion
    }
}
