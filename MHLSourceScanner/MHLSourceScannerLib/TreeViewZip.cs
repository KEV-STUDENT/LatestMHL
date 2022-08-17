using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceOnDisk;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4Zip : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.DarkBlue;
        FontWeight IDecorator.FontWeight => FontWeights.Bold;
        bool IDecorator.Focusable => true;
    }

    public class TreeViewZip : TreeViewDiskItem<Decor4Zip>
    {
        private readonly object sourceLock = new object();

        public int Count
        {
            get
            {
                if (source != null && source is IDiskCollection diskCollection)
                {
                    return diskCollection.Count;
                }
                return 0;
            }
        }

        public int TotalCount
        {
            get
            {
                int cnt = 0;
                if (source != null && source is IDiskCollection diskCollection)
                {
                    foreach (var item in diskCollection.GetChilds())
                    {
                        if(item is IDiskCollection collection)
                            cnt += collection.Count;
                    }
                }
                return cnt;
            }
        }

        public System.Windows.Visibility Visibility2TotalCount
        {
            get { return (TotalCount < IDiskCollection.MaxItemsInVirtualGroup ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible) ; }
        }
        #region [Constructors]
        public TreeViewZip(string path) : base(path)
        {
        }
        public TreeViewZip(string path, IShower? shower) : base(path, shower)
        {
        }

        public TreeViewZip(IDiskItem diskItemSource) : base(diskItemSource)
        {
        }

        public TreeViewZip(IDiskItem diskItemSource, IShower? shower) : base(diskItemSource, shower)
        {
        }
        #endregion

        #region [Protected Methods]
        protected override void LoadItemCollection(IDiskCollection diskCollection)
        {
            if(diskCollection.IsVirtualGroupsUsed)
            {
                base.LoadItemCollection(diskCollection);
            }
            else
            {
                if ((diskCollection?.Count ?? 0) != 0 && !string.IsNullOrEmpty(source?.Path2Item) && (source is DiskItemFileZip fileZip))
                {
                    List<Task> tasks = new List<Task>();
                    List<string> list = new List<string>();

                    using (ZipArchive zipArchive = ZipFile.OpenRead(source.Path2Item))
                    {
                        foreach (ZipArchiveEntry file in zipArchive.Entries)
                            list.Add(file.Name);
                    }

                    foreach (string name in list)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            IDiskItem? diskItemChild = null;
                            lock (sourceLock)
                            {
                                using (ZipArchive zipArchive = ZipFile.OpenRead(source.Path2Item))
                                {
                                    ZipArchiveEntry? file = zipArchive.GetEntry(name);
                                    if (file != null)
                                        diskItemChild = DiskItemFabrick.GetDiskItem(fileZip, file);
                                }
                            }

                            if (diskItemChild != null)
                            {
                                ITreeDiskItem diskItem = this;
                                if (shower == null)
                                    diskItem.AddDiskItem(diskItemChild);
                                else
                                    shower.AddDiskItem(diskItemChild, diskItem);
                            }
                        }));
                    }

                    if (TestMode)
                        Task.WaitAll(tasks.ToArray());
                }
            }
        }
        #endregion
    }
}

