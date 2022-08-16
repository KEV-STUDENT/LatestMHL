using MHLCommon;
using MHLSourceOnDisk;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public struct Decor4VirtualGroup : IDecorator
    {
        Brush IDecorator.ForeGround => Brushes.DarkBlue;
        FontWeight IDecorator.FontWeight => FontWeights.Bold;
        bool IDecorator.Focusable => true;
    }

    public class TreeViewVirtualGroup : TreeViewDiskItem<Decor4VirtualGroup>
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

        #region [Constructors]
        public TreeViewVirtualGroup(string path) : base(path)
        {
        }
        public TreeViewVirtualGroup(string path, IShower? shower) : base(path, shower)
        {
        }

        public TreeViewVirtualGroup(IDiskItem diskItemSource) : base(diskItemSource)
        {
        }

        public TreeViewVirtualGroup(IDiskItem diskItemSource, IShower? shower) : base(diskItemSource, shower)
        {
        }
        #endregion

        #region [Protected Methods]
        protected override void LoadItemCollection(IDiskCollection diskCollection)
        {

            if ((diskCollection?.Count ?? 0) != 0 && !string.IsNullOrEmpty(source?.Path2Item) && 
                (diskCollection is IDiskItemVirtualGroup virtGroup) && 
                (virtGroup.ParentCollection is DiskItemFileZip fileZip))
            {
                List<Task> tasks = new List<Task>();
                foreach (string name in virtGroup.ItemsNames)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        IDiskItem? diskItemChild = null;
                        lock (sourceLock)
                        {
                            using (ZipArchive zipArchive = ZipFile.OpenRead(source.Path2Item))
                            {
                                ZipArchiveEntry? file = zipArchive.GetEntry(name);
                                if(file != null)
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
        #endregion
    }
}

