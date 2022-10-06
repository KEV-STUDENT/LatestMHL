using MHLCommon.MHLBook;
using MHLCommon.MHLScanner;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace MHLSourceScannerLib
{
    public class ShowerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ITreeItem> source;
        private IBook? book;
        public ICommand ExpandingCommand { get; set; }
        public ICommand FB2CheckedCommand { get; set; }

        #region [Properies]
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

        public ObservableCollection<ITreeItem> Source
        {
            get => source;
            set => source = value;
        }
        #endregion

        #region [Constructor]
        public ShowerViewModel()
        {
            ExpandingCommand = new RelayCommand(ExecuteExpandingCommand, CanExecuteExpandingCommand);
            FB2CheckedCommand = new RelayCommand(ExecuteFB2CheckedCommand, CanExecuteFB2CheckedCommand);
            defaultCover = GetImageFromResources("DefaultCover");
            source = new ObservableCollection<ITreeItem>();
        }
        #endregion


        #region [Execute...Command]
        void ExecuteExpandingCommand(object? obj)
        {
            if (obj is RoutedEventArgs arg)
            {
                TreeViewItem? tvi = arg.Source as TreeViewItem;
                if (tvi?.Header is ITreeCollectionItem treeItem)
                {
                    treeItem.LoadChilds();
                }
            }
        }

        void ExecuteFB2CheckedCommand(object? obj)
        {
            if (obj is ITreeItem item)
            {
                if(item.Selected)
                {

                }
                else
                {
                    item.Parent.Selected = false;
                }
            }
        }
        #endregion

        #region [CanExecute...Command]
        bool CanExecuteExpandingCommand(object? obj)
        {
            return true;
        }

        bool CanExecuteFB2CheckedCommand(object? obj)
        {
            return true;
        }
        #endregion

        #region [PropertyChanged]
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            var handler = System.Threading.Interlocked.CompareExchange(ref PropertyChanged, null, null);
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region [Private Methods]
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
        #endregion

        #region [Public Method]
        public void ItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewFB2 fB2)
            {
                Book = fB2.Book;
            }
            else
            {
                Book = null;
            }
        }
        #endregion
    }
}
