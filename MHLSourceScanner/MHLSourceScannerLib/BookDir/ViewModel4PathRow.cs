using MHLCommands;
using MHLCommon.MHLBookDir;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLSourceScannerModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MHLSourceScannerLib.BookDir
{
    public class ViewModel4PathRow : ViewModel
    {
        #region [Fields]
        private IPathRow<PathRowElement> pathRow;
        private bool isSelected;
        #endregion

        #region [Constructors]
        public ViewModel4PathRow(IPathRow<PathRowElement> item)
        {
            pathRow = item;
            AddElementCommand = new RelayCommand(ExecuteAddElementCommand, CanExecuteAddElementCommand);
            DeleteElementCommand = new RelayCommand(ExecuteDeleteElementCommand, CanExecuteDeleteElementCommand);
        }
        #endregion

        #region [Properies]
        public int Count => pathRow.Count;
        public ObservableCollection<PathRowElement> Items => pathRow.Items;
        public bool IsExpanded
        {
            get => pathRow.IsExpanded;
            set
            {
                pathRow.IsExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public bool IsFileName
        {
            get => pathRow.IsFileName;
            set
            {
                pathRow.IsFileName = value;
                OnPropertyChanged("IsFileName");
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set { 
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public bool IsEnabled
        {
            get
            {
                if (pathRow is TreeItemCollection collection)
                {
                    return collection.SourceItems.Count == 0;
                }
                return false;
            }
        }
        public ICommand AddElementCommand { get; set; }
        public ICommand DeleteElementCommand { get; set; }
        #endregion

        #region [Indexer]
        public ViewModel4PathRowElement this[int i]
        {
            get
            {
                return (i >= pathRow.Count ? pathRow[Count].ViewModel : pathRow[i].ViewModel);
            }
        }
        #endregion

        #region [Methods]
        private void ExecuteAddElementCommand(object? obj)
        {
            pathRow.InsertTo(Count);
        }
        private bool CanExecuteAddElementCommand(object? obj)
        {
            return Count < 6;
        }

        private void ExecuteDeleteElementCommand(object? obj)
        {
            pathRow.RemoveFrom(Count);
        }
        private bool CanExecuteDeleteElementCommand(object? obj)
        {
            return Count > 1;
        }
        #endregion
    }
}
