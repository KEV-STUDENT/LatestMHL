using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLControls;
using MHLResources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHLSourceScannerModelLib;
using MHLSourceScannerLib.BookDir;
using MHLCommands;
using System.Windows.Input;

namespace MHLUIElements
{
    public class ViewModel4BookDir : ViewModel
    {
        #region [Fields]
        private ObservableCollection<ITreeItem> source;
        #endregion

        #region [Properies]
        public ObservableCollection<ITreeItem> Source
        {
            get => source;
            set => source = value;
        }
        public ICommand AddRowCommand { get; set; }
        public ICommand DeleteRowCommand { get; set; }
        #endregion

        #region [Constructor]
        public ViewModel4BookDir()
        {
            AddRowCommand = new RelayCommand(ExecuteAddRowCommand, CanExecuteAddRowCommand);
            DeleteRowCommand = new RelayCommand(ExecuteDeleteRowCommand, CanExecuteDeleteRowCommand);

            source = new ObservableCollection<ITreeItem>();
            source.Add(new PathRow());
        }
        #endregion

        #region [Methods]
        private void ExecuteAddRowCommand(object? obj)
        {
            source.Add(new PathRow());
        }
        private bool CanExecuteAddRowCommand(object? obj)
        {
            return true;
        }

        private void ExecuteDeleteRowCommand(object? obj)
        {

        }
        private bool CanExecuteDeleteRowCommand(object? obj)
        {
            return true;
        }
        #endregion
    }
}
