using MHLCommands;
using MHLControls.ViewModels4Forms;
using MHLSourceScanner.Configurations.RowFolder;
using MHLSourceScanner.DirectorySetting;
using MHLSourceScannerLib.BookDir;
using System;
using System.Configuration;
using System.Text.Json;
using System.Windows.Input;
using MHLCommon.ViewModels;
using MHLCommon.MHLBookDir;

namespace MHLSourceScanner.DirectorySettings
{
    public class ViewModel4DirSetting : VMEditForm, IVM4DirSetting
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

            Close += () =>
            {
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
            _model.SaveConfigurations(_view.DirectoryTree.ViewModel.Source[0]);            
        }

        #region [Methods]
        private void ExecuteAddRowCommand(object? obj)
        {
            if (obj is DirSetting form)
            {
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

        internal void LoadDataFromConfig()
        {
            _model.LoadConfigurations((IVM4DirSetting)this);
        }

        private void UpdatePathRowTree(IPathRow row)
        {
            _view.DirectoryTree.ViewModel.Source.Clear();
            if ((row != null) && (row is PathRowVM rowVM))
            {
                _view.DirectoryTree.ViewModel.Source.Add(rowVM);
                while ((rowVM?.SubRows?.Count ?? 0) > 0)
                {
                    if (rowVM != null)
                    {
                        foreach (PathRowVM subRow in rowVM.SubRows)
                        {
                            subRow.Parent = rowVM;
                        }
                        rowVM = rowVM.SubRows[0] as PathRowVM;
                    }
                }
                if (rowVM != null)
                    rowVM.ViewModel.IsSelected = true;
            }
        }
        #endregion

        #region [IVM4DirSetting Implementation]
        void IVM4DirSetting.UpdatePathRowTree(IPathRow row)
        {
            UpdatePathRowTree(row);
        }
        #endregion
    }
}
