using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MHLSourceScannerLib
{
    /// <summary>
    /// Логика взаимодействия для SourceTree.xaml
    /// </summary>
    public partial class SourceTree : UserControl, IShower
    {
        public event SelectionChanged SelectedItemChanged;
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
            ShowSource.SelectedItemChanged += ItemChanged;
        }

        private void ItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(e.NewValue is ITreeItem treeItem)
            {
                if(treeItem != null)
                    SelectedItemChanged(treeItem);
            }
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
