using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

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
            shower.LoadCollection = LoadItemCollection;
            this.shower = shower;
            ViewModel = new ShowerViewModel();
            ShowSource.ItemsSource = ViewModel.Source;
            ShowSource.SelectedItemChanged += ViewModel.ItemChanged;          
        }

        private void UpdateViewAction()
        {
            ShowSource.ItemsSource = SourceItems;
            ViewModel.Source = SourceItems;
        }

        private void CreateViewItem(IDiskItem item, ITreeDiskItem parent)
        {
            parent.AddDiskItem(item);
        }

        private void LoadItemCollection(ITreeCollectionItem parent)
        {
            if (parent is ITreeDiskItem diskItem)
                diskItem.SourceItems.Clear();
            parent.LoadItemCollection();
        }

        public ObservableCollection<ITreeItem> SourceItems
        {
            get { return shower.SourceItems; }
        }


        #region [IShower implementation]
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
        #endregion
    }
}
