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
    public class Decor4VirtualGroup : Decorator4WPF
    {
        public override Brush ForeGround => Brushes.DarkBlue;
        public override FontWeight FontWeight => FontWeights.Bold;
        public override bool Focusable => true;
        public override bool ThreeState => true;
    }

    public class TreeViewVirtualGroup : TreeViewDiskItem<Decor4VirtualGroup, ViewModel4TreeItem>
    {
        private readonly object sourceLock = new object();

        public int Count
        {
            get
            {
                if (Source != null && Source is IDiskCollection diskCollection)
                {
                    return diskCollection.Count;
                }
                return 0;
            }
        }

        #region [Constructors]
        public TreeViewVirtualGroup(string path, ITreeItem? parent) : base(path, parent)
        {
        }
        public TreeViewVirtualGroup(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewVirtualGroup(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }

        public TreeViewVirtualGroup(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
        #endregion

        #region [Protected Methods]
        protected override void LoadItemCollection(IDiskCollection diskCollection)
        {

            if ((diskCollection?.Count ?? 0) != 0 && !string.IsNullOrEmpty(Source?.Path2Item) &&
                (diskCollection is IVirtualGroup virtGroup) &&
                (virtGroup.ParentCollection is DiskItemFileZip fileZip))
            {
                Parallel.ForEach(virtGroup.ItemsNames, name =>
                {
                    IDiskItem? diskItemChild = null;
                    ITreeItem newItem;

                    using (ZipArchive zipArchive = ZipFile.OpenRead(Source.Path2Item))
                    {
                        ZipArchiveEntry? file = zipArchive.GetEntry(name);
                        if (file != null)
                        {
                            diskItemChild = DiskItemFabrick.GetDiskItem(fileZip, file);
                        }
                    }

                    if (diskItemChild != null) {
                        newItem = CreateTreeViewItem(diskItemChild);
                        if (!Insert2SourceLock(newItem))
                            Add2SourceLock(newItem);

                    }
                });
            }
        }
        #endregion
    }
}

