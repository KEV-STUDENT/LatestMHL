using MHLCommon.ViewModels;
using System.Windows.Input;
using MHLSourceScannerModelLib;
using MHLCommon.MHLScanner;

namespace MHLSourceScannerLib
{
    public class ViewModel4TreeItem : ViewModel, ISelected
    {
        private bool? selected = false;
        private bool prevSelected = false;
        public bool? IsSelected
        {
            get => selected;
            set
            {
                prevSelected = selected??prevSelected;

                selected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        bool? ISelected.IsSelected
        {
            get => IsSelected;
            set => IsSelected = value;
        }

        public ICommand CheckedCommand { get; set; }

        #region[Constructors]
        public ViewModel4TreeItem()
        {
            CheckedCommand = new RelayCommand(ExecuteCheckedCommand, CanExecuteCheckedCommand);
        }
        #endregion

        #region [Private Methodth]
        private void ExecuteCheckedCommand(object? obj)
        {
            if (obj is ITreeItem treeItem)
            {
                if(treeItem.Parent != null)
                   treeItem.Selected = !prevSelected;
            }

            if(obj is ITreeCollectionItem collectionItem)
            {
                foreach(ITreeItem item in collectionItem.SourceItems)
                {
                    item.Selected = collectionItem.Selected;
                }
            }
        }
        private bool CanExecuteCheckedCommand(object? obj)
        {
            return true;
        }
        #endregion

    }
}
