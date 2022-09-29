using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;


using System.Drawing;
using System.IO;
using System.Resources;

using System.Windows.Media.Imaging;
using System.Collections;

namespace MHLSourceScannerLib
{
    /// <summary>
    /// Логика взаимодействия для SourceTree.xaml
    /// </summary>
    public partial class SourceTree : UserControl, IShower, INotifyPropertyChanged
    {
        //public event SelectionChanged? SelectedItemChanged;
        public ShowerViewModel ViewModel { get; private set; }

        private readonly BitmapImage defaultCover;

        public IBook? Book
        {
            get => book;
            private set
            {
                bool annotationSectionHeightChanged = (book != null && value == null) || (book == null && value != null);
                book = value;
                if (annotationSectionHeightChanged)
                    OnPropertyChanged("AnnotationSectionHeigh");

                if (book != null)
                {
                    OnPropertyChanged("Annotation");
                    OnPropertyChanged("Cover");
                    OnPropertyChanged("Authors");
                }
            }
        }
        public string Annotation
        {
            get
            {
                return book?.Annotation ?? string.Empty;
            }
        }

        public string Authors
        {
            get
            {
                string authors = string.Empty;
                string authorName;

                if ((book?.Authors?.Count ?? 0) > 0)
                    foreach (MHLAuthor author in book.Authors)
                    {
                        authorName = author.LastName.Trim();
                        authorName = String.Format("{0} {1}", authorName, author.FirstName.Trim());
                        authorName = String.Format("{0} {1}", authorName, author.MiddleName.Trim());


                        if (!string.IsNullOrEmpty(authorName))
                            if (string.IsNullOrEmpty(authors))
                                authors = authorName;
                            else
                                authors = String.Format("{0}, {1}", authors.Trim(), authorName.Trim());
                    }

                return authors;
            }
        }

        public BitmapImage? Cover
        {
            get
            {
                if (book == null || string.IsNullOrEmpty(book.Cover))
                    return defaultCover;

                byte[] plainTextBytes = System.Convert.FromBase64String(book.Cover);

                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(plainTextBytes))
                {
                    ms.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
                return bitmapImage;
            }
        }

        public int AnnotationSectionHeigh
        {
            get
            {
                return (book == null ? 0 : 100);
            }
        }

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

            defaultCover = GetImageFromResources("DefaultCover");
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
                //string? Cover = Book.Cover;
            }
            else
            {
                Book = null;
            }
        }

        private BitmapImage GetImageFromResources(string resName)
        {
            BitmapImage bitmapImage = new BitmapImage();

            ResourceManager resourceManager = new ResourceManager("MHLSourceScannerLib.LibResources", typeof(SourceTree).Assembly);
            Bitmap? df = resourceManager.GetObject(resName) as Bitmap;
            if (df != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    df.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
            }
            return bitmapImage;
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
