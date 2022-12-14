using MHLCommands;
using MHLControls.ViewModels4Forms;
using System;
using System.Windows.Input;

namespace MHLSourceScanner
{
    public class ViewModel4Scanner : VMEditForm
    {
        private bool _destinationIsDirectory = true;

        #region [Constructors]
        public ViewModel4Scanner():base()
        {
            SetDestinationDirCommand = new RelayCommand(ExecuteSetDestinationDirCommand, CanExecuteSetDestinationDirCommand);
            ChangeDestinationDirCommand = new RelayCommand(ExecuteChangeDestinationDirCommand, CanExecuteChangeDestinationDirCommand);
            ChangeSourceDirCommand = new RelayCommand(ExecuteChangeSourceDirCommand, CanExecuteChangeSourceDirCommand);
        }
        #endregion

        #region [Properties]
        public Action? SetDestinationDirAction;
        public Action? ChangeDestinationDirAction;
        public Action? ChangeSourceDirAction;
        public ICommand SetDestinationDirCommand { get; set; }
        public ICommand ChangeDestinationDirCommand { get; set; }
        public ICommand ChangeSourceDirCommand { get; set; }

        public bool DestinationIsDirectory
        {
            get { return _destinationIsDirectory; }
            set
            {
                _destinationIsDirectory = value;
                OnPropertyChanged("DestinationIsDBFile");
                OnPropertyChanged("DestinationIsDirectory");
            }
        }

        public bool DestinationIsDBFile
        {
            get { return !_destinationIsDirectory; }
            set
            {
                _destinationIsDirectory = !value;
                OnPropertyChanged("DestinationIsDBFile");
                OnPropertyChanged("DestinationIsDirectory");
            }
        }
        #endregion

        #region [Private Methods]
        private void ExecuteSetDestinationDirCommand(object? obj)
        { SetDestinationDirAction?.Invoke();}
        private bool CanExecuteSetDestinationDirCommand(object? obj)
        { return SetDestinationDirAction != null; }

        private void ExecuteChangeDestinationDirCommand(object? obj)
        { ChangeDestinationDirAction?.Invoke(); }

        private bool CanExecuteChangeDestinationDirCommand(object? obj)
        { return ChangeDestinationDirAction != null; }

        private void ExecuteChangeSourceDirCommand(object? obj)
        { ChangeSourceDirAction?.Invoke(); }

        private bool CanExecuteChangeSourceDirCommand(object? obj)
        { return ChangeSourceDirAction != null; }



        #endregion
    }
}
