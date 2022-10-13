using MHLCommon;
using MHLCommon.MHLScanner;
using MHLSourceScannerLib;
using System.ComponentModel;
using System.Windows;


namespace MHLSourceScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SourceDirectoryPicker.PropertyChanged += SourceDirectoryPickerChanged;
        }


        private void SourceDirectoryPickerChanged(object? sender, PropertyChangedEventArgs e)
        {
            string directory;

            if( ((IPicker<string>)SourceDirectoryPicker).CheckValue(out directory) == ReturnResultEnum.Ok)
            {
                IShower shower = SourceDirectoryTree;
                ITreeDiskItem treeViewDiskItem = new TreeViewDirectory(directory, shower, null);
                shower.SourceItems.Clear();
                shower.UpdateView(treeViewDiskItem);
            }
        }
    }
}
