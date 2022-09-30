using MHLCommon.MHLScanner;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MHLSourceScannerLib
{
    public class ShowerViewModel: INotifyPropertyChanged
    {

        public ICommand ExpandingCommand { get; set; }
        public ICommand FB2CheckedCommand {get; set; }

        public ShowerViewModel()
        {
            ExpandingCommand = new RelayCommand(ExecuteExpandingCommand, CanExecuteExpandingCommand);
            FB2CheckedCommand = new RelayCommand(ExecuteFB2CheckedCommand, CanExecuteFB2CheckedCommand);
        }
        void ExecuteExpandingCommand(object obj)
        {
            if(obj is RoutedEventArgs arg)
            {
                TreeViewItem? tvi = arg.Source as TreeViewItem;
                if(tvi?.Header is ITreeCollectionItem treeItem)
                {
                    treeItem.LoadChilds();
                }
            }
        }

        void ExecuteFB2CheckedCommand(object obj)
        {
            if (obj is RoutedEventArgs arg)
            {
               /* TreeViewItem? tvi = arg.Source as TreeViewItem;
                if (tvi?.Header is ITreeCollectionItem treeItem)
                {
                    treeItem.LoadChilds();
                }*/
            }
        }

        bool CanExecuteExpandingCommand(object obj)
        {
            return true;
        }

        bool CanExecuteFB2CheckedCommand(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            var handler = System.Threading.Interlocked.CompareExchange(ref PropertyChanged, null, null);
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(prop));
        }
    }
}
