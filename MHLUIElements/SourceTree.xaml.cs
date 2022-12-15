using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLResources;
using MHLSourceScannerModelLib;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

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

        public ObservableCollection<ITreeItem> SourceItems
        {
            get { return sourceItems; }
        }


        #region [IShower implementation]
        ObservableCollection<ITreeItem> IShower.SourceItems
        {
            get { return SourceItems; }
        }

        void IShower.UpdateView(ITreeItem treeItem)
        {
            if (treeItem is ITreeDiskItem treeViewDiskItem)
            {
                treeViewDiskItem.ClearCollection();
                treeViewDiskItem.LoadChildsAsync();
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

        void IShower.Clear(ITreeItemCollection treeItem)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                treeItem.ClearCollection();
            }));
        }

        void IShower.Add2Source(ITreeItem item, ITreeItemCollection treeView)
        {
            System.Diagnostics.Debug.WriteLine("Shower Add : " + item.Name);
            Dispatcher.InvokeAsync(new Action(() =>
            {
                treeView.Add2Source(item);
            }));
        }

        bool IShower.Insert2Source(ITreeItem item, ITreeItemCollection treeView)
        {
            System.Diagnostics.Debug.WriteLine("Shower Insert : " + item.Name);

            Dispatcher.InvokeAsync(new Action(() =>
            {
                if(!treeView.Insert2Source(item))
                    treeView.Add2Source(item);
            }));
            return true;
        }
        #endregion
    }
}
