using MHLCommands;
using MHLCommon.MHLBook;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLResources;
using MHLSourceScannerLib;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using MHLSourceOnDisk;
using MHLCommon;
using MHLSourceScannerModelLib;
using MHLCommon.MHLDiskItems;
using System.Linq;

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
                    foreach (MHLAuthor author in book.Authors.Cast<MHLAuthor>())
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
                if (tvi?.Header is ITreeItemCollection treeItem)
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

        public async Task ExportSelectedItemsAsync(ObservableCollection<ITreeItem> collection, Export2Dir exporter)
        {
            await Parallel.ForEachAsync<ITreeItem>(collection, async (item, cancelToken) =>
            {
                string name;
                bool? continueExport;
                continueExport = CheckItem4Export(item);
                if(continueExport ?? true)
                {
                    name = item.Name;
                    if (item is TreeDiskItem diskItem)
                    {
                        if (continueExport ?? false)
                        {
                            await ExportSelectedDiskItemAsync(diskItem.Source, exporter);
                        }
                        else
                        {
                            await ExportSelectedItemsAsync(diskItem.SourceItems, exporter);
                        }
                    }
                }              
            });
        }

        public void ExportSelectedItems(ObservableCollection<ITreeItem> collection, Export2Dir exporter)
        {
            bool? continueExport;
            string name;

            foreach (ITreeItem item in collection)
            {
                continueExport = CheckItem4Export(item);
                if (continueExport??true)
                {
                    name = item.Name;
                    if (item is TreeDiskItem diskItem)
                    {
                        if (continueExport ?? false)
                            ExportSelectedDiskItem(diskItem.Source, exporter);
                        else
                            ExportSelectedItems(diskItem.SourceItems, exporter);
                    }
                }
            }
        }

        private static bool? CheckItem4Export(ITreeItem item)
        {
            bool? result;

            if (item is TreeViewZip zip)
            {
                result = zip.Selected;
            }
            else if (item is TreeViewVirtualGroup vg)
            {
                result = vg.Selected;
            }
            else if (item is TreeViewFB2 fb2)
            {
                result = (fb2.Selected ?? false);
            }
            else if (item is TreeViewDirectory dir)
            {
                result = (dir.ChildsLoaded && dir.SourceItems.Count > 0 ? null : false);
            }
            else
            {
                result = false;
            }

            return result;
        }

        private static void ExportSelectedDiskItem(IDiskItem? diskItem, Export2Dir exporter)
        {
            string name;
            if (diskItem != null)
            {
                name = diskItem.Name;
                diskItem.ExportBooks(exporter);
            }
        }

        private static async Task ExportSelectedDiskItemAsync(IDiskItem? diskItem, Export2Dir exporter)
        {
           await Task.Run(() => ExportSelectedDiskItem(diskItem, exporter));
        }
        #endregion
    }
}
