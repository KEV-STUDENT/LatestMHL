using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MHLUIElements
{
    /// <summary>
    /// Логика взаимодействия для SourceTree.xaml
    /// </summary>
    public partial class SourceTree : UserControl, IShower
    {
        protected ObservableCollection<ITreeItem> sourceItems = new ObservableCollection<ITreeItem>();
        public ViewModel4Shower ViewModel { get; }

        public SourceTree()
        {
            ViewModel = new ViewModel4Shower();

            InitializeComponent();
            
            ShowSource.ItemsSource = ViewModel.Source;
            ShowSource.SelectedItemChanged += ViewModel.ItemChanged;          
        }

        private void UpdateViewAction()
        {
            ShowSource.ItemsSource = SourceItems;
            ViewModel.Source = SourceItems;
        }

        private void LoadItemCollection(ITreeItemCollection parent)
        {
            Clear(parent);
            parent.LoadItemCollection();
        }

        private void Clear(ITreeItemCollection parent)
        {
            if (parent is ITreeDiskItem diskItem)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    diskItem.SourceItems.Clear();
                }));
            }
        }

        public ObservableCollection<ITreeItem> SourceItems
        {
            get { return sourceItems; }
        }


        #region [IShower implementation]
        ObservableCollection<ITreeItem> IShower.SourceItems
        {
            get { return SourceItems; }
        }

        void IShower.UpdateView()
        {
            UpdateViewAction();
        }

        void IShower.UpdateView(ITreeItem treeItem)
        {
            if (treeItem is ITreeDiskItem treeViewDiskItem)
            {
                treeViewDiskItem.LoadChilds();
                sourceItems = treeViewDiskItem.SourceItems;
            }
            UpdateViewAction();
        }

        void IShower.AddDiskItem(IDiskItem diskItem, ITreeDiskItem treeItem)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                treeItem.AddDiskItem(diskItem);
            }));
        }

        void IShower.LoadItemCollection(ITreeItemCollection treeItem)
        {
            LoadItemCollection(treeItem);
        }

        void IShower.Clear(ITreeItemCollection treeItem)
        {
            Clear(treeItem);
        }
        #endregion
    }
}
