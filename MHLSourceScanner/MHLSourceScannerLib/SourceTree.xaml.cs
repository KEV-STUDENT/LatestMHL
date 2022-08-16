using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MHLSourceOnDisk;
using MHLSourceScannerModelLib;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using MHLCommon;

namespace MHLSourceScannerLib
{
    /// <summary>
    /// Логика взаимодействия для SourceTree.xaml
    /// </summary>
    public partial class SourceTree : UserControl, IShower
    {
        public ShowerViewModel ViewModel { get; private set; }
        protected IShower shower;
        public SourceTree()
        {
            InitializeComponent();
            DiskItemShower shower = new DiskItemShower();
            shower.UpdateView = UpdateViewAction;
            //shower.CreateItem = CreateViewItem;
            shower.LoadCollection = LoadItemCollection;
            this.shower = shower;
            ViewModel = new ShowerViewModel();
            //DataContext = this;
            ShowSource.ItemsSource = new ObservableCollection<ITreeItem>();
        }


        private void UpdateViewAction()
        {
            ShowSource.ItemsSource = SourceItems;
        }

        private void CreateViewItem(IDiskItem item, ITreeDiskItem parent)
        {
            parent.AddDiskItem(item);
        }

        private void LoadItemCollection(ITreeCollectionItem parent)
        {
            if(parent is ITreeDiskItem diskItem)
                diskItem.SourceItems.Clear();
            parent.LoadItemCollection();
        }

        public ObservableCollection<ITreeItem> SourceItems
        {
            get { return shower.SourceItems; }
        }


        ObservableCollection<ITreeItem> IShower.SourceItems
        {
            get { return SourceItems; }
        }

        void IShower.UpdateView()
        {
            shower.UpdateView();
        }

        void IShower.UpdateView(ITreeItem treeItem)
        {
            shower.UpdateView(treeItem);
        }

        void IShower.AddDiskItem(IDiskItem diskItem, ITreeDiskItem treeItem)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                treeItem.AddDiskItem(diskItem);
            }));
        }

        void IShower.LoadItemCollection(ITreeCollectionItem treeItem)
        {
            shower.LoadItemCollection(treeItem);
        }
    }
}
