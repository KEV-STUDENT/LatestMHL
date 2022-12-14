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
    public class ViewModel4PathElement : ViewModel
    {
        #region [Fields]
        private PathElementVM pathElement;
        #endregion

        #region [Constructors]
        public ViewModel4PathElement(PathElementVM pathElement)
        {
            this.pathElement = pathElement;
            TypeChangedCommand = new RelayCommand(ExecuteTypeChangedCommand, CanExecuteTypeChangedCommand);
        }
        #endregion

        #region [Properties]
        public ICommand TypeChangedCommand { get; set; }
        public ObservableCollection<ElementTypeUI> Source => pathElement.Source;
        public ElementTypeUI SelectedItem
        {
            get => pathElement.SelectedItem;
            set
            {
                pathElement.SelectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public bool IsTyped => pathElement.IsTyped;
        public string Name => pathElement.Name;
        #endregion

        #region [Private Methodth]
        private void ExecuteTypeChangedCommand(object? obj)
        {
            //Type Changed
        }
        private bool CanExecuteTypeChangedCommand(object? obj)
        {
            return IsTyped;
        }
        #endregion
    }
}
