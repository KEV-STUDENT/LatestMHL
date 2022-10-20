using MHLCommon.MHLBook;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MHLSourceScannerLib;
using MHLControls;
using MHLResources;


namespace MHLUIElements
{
    public class ViewModel4Shower : ViewModel
    {
        private ObservableCollection<ITreeItem> source;
        private IBook? book;
        private readonly BitmapImage defaultCover;

        #region [Properies]
        public ICommand ExpandingCommand { get; set; }

        public IBook? Book
        {
            get => book;
            private set
            {
                bool annotationSectionHeightChanged = book != null && value == null || book == null && value != null;
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
                        authorName = string.Format("{0} {1}", authorName, author.FirstName.Trim());
                        authorName = string.Format("{0} {1}", authorName, author.MiddleName.Trim());


                        if (!string.IsNullOrEmpty(authorName))
                            if (string.IsNullOrEmpty(authors))
                                authors = authorName;
                            else
                                authors = string.Format("{0}, {1}", authors.Trim(), authorName.Trim());
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

                byte[] plainTextBytes = Convert.FromBase64String(book.Cover);

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
                return book == null ? 0 : 100;
            }
        }

        public ObservableCollection<ITreeItem> Source
        {
            get => source;
            set => source = value;
        }
        #endregion

        #region [Constructor]
        public ViewModel4Shower()
        {
            ExpandingCommand = new RelayCommand(ExecuteExpandingCommand, CanExecuteExpandingCommand);
            defaultCover = MHLResourcesManager.GetImageFromResources("DefaultBookCover");
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
        #endregion

        #region [CanExecute...Command]
        bool CanExecuteExpandingCommand(object? obj)
        {
            return true;
        }
        #endregion

        #region [Private Methods]
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
