using MHLCommands;
using MHLCommon.MHLBookDir;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MHLSourceScannerLib.BookDir
{
    public class ViewModel4PathRowElement:ViewModel
    {
        #region [Fields]
        private PathRowElement pathRowElement;
        #endregion


        #region [Constructors]
        public ViewModel4PathRowElement(PathRowElement pathRowElement)
        {
            this.pathRowElement = pathRowElement;
            ElementChangedCommand = new RelayCommand(ExecuteElementChangedCommand, CanExecuteElementChangedCommand);
        }
        #endregion

        #region [Properties]
        public ObservableCollection<PathElement> Source => pathRowElement.Source;
        public PathElement SelectedItem
        {
            get => pathRowElement.SelectedItem;
            set
            {
                pathRowElement.SelectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public ICommand ElementChangedCommand { get; set; }
        #endregion

        #region [Private Methodth]
        private void ExecuteElementChangedCommand(object? obj)
        {
            //Element Changed
        }
        private bool CanExecuteElementChangedCommand(object? obj)
        {
            return true;
        }
        #endregion
    }
}
