using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MHLSourceScannerLib
{
    /// <summary>
    /// Логика взаимодействия для SourceTree.xaml
    /// </summary>
    public partial class SourceTree : UserControl, IShower, INotifyPropertyChanged
    {
        public event SelectionChanged SelectedItemChanged;
        public ShowerViewModel ViewModel { get; private set; }
        public IBook? Book {
            get => book;
            private set {
                book = value;
                OnPropertyChanged("Annotation");
            } 
        }
        public string Annotation { get => book?.Annotation ?? string.Empty;}
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected IShower shower;
        private IBook? book = null;
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

        event PropertyChangedEventHandler? PropertyChanged;
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                this.PropertyChanged += value;
            }

            remove
            {
                PropertyChanged -= value;
            }
        }

        private void ItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewFB2 fB2)
            {
                Book = fB2.Book;
                string? Cover = Book.Cover;
            }
            else
            {
                Book = null;
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
