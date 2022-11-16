using MHLCommon.MHLBook;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MHLSourceScannerLib;
using MHLControls;

namespace MHLSourceScanner
{
    public class ViewModel4Scanner : ViewModel
    {
        private bool _destinationIsDirectory = true;

        #region [Constructors]
        public ViewModel4Scanner()
        {
            CloseCommand = new RelayCommand(ExecuteCloseCommand, CanExecuteCloseCommand);
            RunCommand = new RelayCommand(ExecuteRunCommand, CanExecuteRunCommand);
            SetDestinationDirCommand = new RelayCommand(ExecuteSetDestinationDirCommand, CanExecuteSetDestinationDirCommand);
        }
        #endregion

        #region [Properties]
        public Action? CloseAction;
        public Action? RunAction;
        public Action? SetDestinationDirAction;
        public ICommand CloseCommand { get; set; }
        public ICommand RunCommand { get; set; }
        public ICommand SetDestinationDirCommand { get; set; }

        public bool DestinationIsDirectory
        {
            get { return _destinationIsDirectory; }
            set {
                _destinationIsDirectory = value;
                OnPropertyChanged("DestinationIsDBFile");
                OnPropertyChanged("DestinationIsDirectory");
            }
        }

        public bool DestinationIsDBFile
        {
            get { return !_destinationIsDirectory; }
            set { 
                _destinationIsDirectory = !value;
                OnPropertyChanged("DestinationIsDBFile");
                OnPropertyChanged("DestinationIsDirectory");
            }
        }
        #endregion

        #region [Private Methods]
        private void ExecuteCloseCommand(object? obj)
        {
            CloseAction?.Invoke();
        }
        private bool CanExecuteCloseCommand(object? obj)
        {
            return CloseAction != null;
        }

        private void ExecuteRunCommand(object? obj)
        {
            RunAction?.Invoke();
        }
        private bool CanExecuteRunCommand(object? obj)
        {
            return RunAction != null;
        }

        private void ExecuteSetDestinationDirCommand(object? obj)
        {
            SetDestinationDirAction?.Invoke();
        }
        private bool CanExecuteSetDestinationDirCommand(object? obj)
        {
            return SetDestinationDirAction != null;
        }
        #endregion
    }
}
