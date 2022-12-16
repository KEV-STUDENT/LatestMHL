using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLSourceScannerModelLib;
using System.Linq;
using System.Windows.Input;
using MHLCommands;
using MHLResources;
using System.Windows.Media.Imaging;

namespace MHLSourceScannerLib
{
    public class ViewModel4TreeItem : ViewModel, ISelected
    {
        private BitmapImage _exportImage = MHLResourcesManager.GetImageFromResources("check_12x12");
        private bool? selected = false;
        private bool prevSelected = false;
        private bool isExported = false;
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

        public BitmapImage ExportImage
        {
            get => _exportImage;
        }

        public bool IsExported
        {
            get => isExported;
            set
            {
                isExported = value;
                OnPropertyChanged("IsExported");
            }
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
            if (obj is IItemSelected itemSelected)
            {
                if (obj is ITreeItem treeItem)
                {
                    if (treeItem.Parent != null)
                        itemSelected.Selected = !prevSelected;
                }

                if (obj is TreeItemCollection collectionItem)
                {
                    foreach (TreeItem item in collectionItem.SourceItems)
                    {
                        if(item is IItemSelected selected)
                            selected.Selected = itemSelected.Selected;
                    }
                }
            }
        }
        private bool CanExecuteCheckedCommand(object? obj)
        {
            return true;
        }
        #endregion

        #region [ISelected Implementation]
        bool? ISelected.IsSelected
        {
            get => IsSelected;
            set => IsSelected = value;
        }

        bool ISelected.IsExported
        {
            get => IsExported;
            set => IsExported = value;
        }

        void ISelected.SetParentSelected(ITreeItem? parent, bool? value)
        {
            if ((parent is IItemSelected _parentSelected) && _parentSelected != null && !(_parentSelected.Selected == null && value == null) && _parentSelected.Selected != value)
            {
                if (parent is ITreeItemCollection collectionItem)
                {
                    var p = from a in collectionItem.SourceItems
                            where !(_parentSelected.Selected == null && value == null) && (a is IItemSelected selected) && selected?.Selected != value
                            select a;

                    if (p.Any())
                        _parentSelected.Selected = null;
                    else
                        _parentSelected.Selected = value;
                }
            }
        }

        void ISelected.SetSelecetdFromParent(ITreeItem? parent)
        {
            if ((parent is IItemSelected itemSelected) && (itemSelected?.Selected != null))
                IsSelected = itemSelected.Selected;
        }

        void ISelected.SetParentExported(ITreeItem? parent, bool value)
        {
            if ((parent is IItemSelected _parentSelected) && _parentSelected.IsExported != value)
            {
                if (parent is ITreeItemCollection collectionItem)
                {
                    var p = from a in collectionItem.SourceItems
                            where (a is IItemSelected selected) && selected.IsExported != value
                            select a;

                    if (p.Any())
                        _parentSelected.IsExported = false;
                    else
                        _parentSelected.IsExported = value;
                }
            }
        }

        void ISelected.SetExportedFromParent(ITreeItem? parent)
        {
            if (parent is IItemSelected itemSelected)
                IsExported = itemSelected.IsExported;
        }
        #endregion
    }
}
