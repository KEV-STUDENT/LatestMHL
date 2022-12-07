using MHLCommands;
using MHLControls.ViewModels4Forms;
using MHLSourceScannerLib.BookDir;
using System.Windows.Input;

namespace MHLSourceScanner.DirectorySettings
{
    public class ViewModel4DirSetting : VMEditForm
    {
        #region [Properies]
        public ICommand AddRowCommand { get; set; }
        public ICommand DeleteRowCommand { get; set; }
        #endregion

        public ViewModel4DirSetting() : base()
        {
            AddRowCommand = new RelayCommand(ExecuteAddRowCommand, CanExecuteAddRowCommand);
            DeleteRowCommand = new RelayCommand(ExecuteDeleteRowCommand, CanExecuteDeleteRowCommand);
        }

        #region [Methods]
        private void ExecuteAddRowCommand(object? obj)
        {
            if(obj is DirSetting form) {
                if(form.DirectoryTree.SelectedItem is PathRow selectedRow)
                {
                    PathRow newRow = new PathRow(selectedRow);
                    newRow.IsFileName = selectedRow.IsFileName;
                    selectedRow.SubRows.Add(newRow);
                    selectedRow.ViewModel.IsFileName = false;
                    selectedRow.ViewModel.IsExpanded = true;
                    selectedRow.ViewModel.OnPropertyChanged("IsEnabled");
                    newRow.ViewModel.IsSelected = true;
                }
            }
        }
        private bool CanExecuteAddRowCommand(object? obj)
        {
            if (obj is DirSetting form)
            {
                if (form.DirectoryTree.SelectedItem is PathRow selectedRow)
                {
                   return selectedRow.SubRows.Count == 0;
                }
            }
            return false;
        }

        private void ExecuteDeleteRowCommand(object? obj)
        {
            if (obj is DirSetting form)
            {
                if (form.DirectoryTree.SelectedItem is PathRow selectedRow)
                {
                    if(selectedRow.Parent is PathRow row)
                    {
                        row.SubRows.Remove(selectedRow);
                        row.ViewModel.IsFileName = selectedRow.IsFileName;
                        row.ViewModel.OnPropertyChanged("IsEnabled");
                    }
                }
            }

        }
        private bool CanExecuteDeleteRowCommand(object? obj)
        {
            if (obj is DirSetting form)
            {
                return form.DirectoryTree.SelectedItem is PathRow;
            }
            return false;
        }
        #endregion
    }
}
