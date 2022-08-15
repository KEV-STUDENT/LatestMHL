using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MHLSourceScannerLib;
using MHLSourceScannerModelLib;
using MHLSourceOnDisk;
using MHLCommon;

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
                IShower shower = SourceTree;
                ITreeDiskItem treeViewDiskItem = new TreeViewDirectory(directory,shower);
                shower.SourceItems.Clear();
                shower.UpdateView(treeViewDiskItem);
            }
        }
    }
}
