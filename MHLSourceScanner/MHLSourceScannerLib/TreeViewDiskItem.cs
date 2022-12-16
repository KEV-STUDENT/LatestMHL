using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLSourceOnDisk;
using MHLSourceScannerModelLib;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MHLSourceScannerLib
{
    public class TreeViewUnknown : TreeViewDiskItem<Decor4Unknown>
    {
        public TreeViewUnknown(string path, ITreeItem? parent) : base(path, parent)
        {
        }

        public TreeViewUnknown(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewUnknown(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }

        public TreeViewUnknown(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
    }
    public class TreeViewSystem : TreeViewDiskItem<Decor4System>
    {
        public TreeViewSystem(string path, ITreeItem? parent) : base(path, parent)
        {
        }
        public TreeViewSystem(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewSystem(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }
        public TreeViewSystem(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
    }

    public class TreeViewError : TreeViewDiskItem<Decor4Error>
    {
        public TreeViewError(ITreeItem? parent) : base(parent)
        {
        }
        public TreeViewError(string path, ITreeItem? parent) : base(path, parent)
        {
        }

        public TreeViewError(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewError(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }

        public TreeViewError(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }
    }

    public abstract class TreeViewDiskItem<T> : TreeDiskItem<T> where T : Decorator4WPF, new()
    {
        public TreeViewDiskItem(ITreeItem? parent) : base(parent)
        {
        }
        public TreeViewDiskItem(IShower? shower, ITreeItem? parent) : base(shower, parent)
        {
        }

        public TreeViewDiskItem(string path, ITreeItem? parent) : base(path, parent)
        {
        }
        public TreeViewDiskItem(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent)
        {
        }

        public TreeViewDiskItem(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent)
        {
        }
        public TreeViewDiskItem(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent)
        {
        }

        public Brush ForeGround
        {
            get => ((IDecorator4WPF)Decor).ForeGround;
        }

        public FontWeight FontWeight
        {
            get => ((IDecorator4WPF)Decor).FontWeight;
        }

        public bool Focusable
        {
            get => Decor.Focusable;
        }

        public bool ThreeState
        {
            get => Decor.ThreeState;
        }


        public override ITreeDiskItem CreateTreeViewItem(IDiskItem diskItemChild)
        {
            ITreeDiskItem newTreeItem;
            System.Diagnostics.Debug.WriteLine("TreeItem :" + diskItemChild.Name);
            if (diskItemChild is IBook)
            {
                newTreeItem = new TreeViewFB2(diskItemChild, shower, this);
                _ = ((TreeViewFB2)newTreeItem).Title;
            }
            else if (diskItemChild is DiskItemFileZip)
                newTreeItem = new TreeViewZip(diskItemChild, shower, this);
            else if (diskItemChild is IVirtualGroup)
                newTreeItem = new TreeViewVirtualGroup(diskItemChild, shower, this);
            else if (diskItemChild is DiskItemDirectorySystem || diskItemChild is DiskItemFileSystem)
                newTreeItem = new TreeViewSystem(diskItemChild, shower, this);
            else if (diskItemChild is DiskItemDirectory)
                newTreeItem = new TreeViewDirectory(diskItemChild, shower, this);
            else if (diskItemChild is DiskItemError)
                newTreeItem = new TreeViewError(diskItemChild, shower, this);
            else newTreeItem = new TreeViewUnknown(diskItemChild, shower, this);

            return newTreeItem;
        }
    }

    public abstract class TreeViewDiskItem<T1, T2> : TreeViewDiskItem<T1>, IItemSelected
        where T1 : Decorator4WPF, new()
        where T2 : ISelected, new()
    {
        #region [Fields]
        private T2 viewModel;
        #endregion

        #region [Constructors]
        public TreeViewDiskItem(ITreeItem? parent) : base(parent) { }

        public TreeViewDiskItem(IShower? shower, ITreeItem? parent) : base(shower, parent) { }

        public TreeViewDiskItem(string path, ITreeItem? parent) : base(path, parent) { }
        public TreeViewDiskItem(string path, IShower? shower, ITreeItem? parent) : base(path, shower, parent) { }

        public TreeViewDiskItem(IDiskItem diskItemSource, ITreeItem? parent) : base(diskItemSource, parent) { }

        public TreeViewDiskItem(IDiskItem diskItemSource, IShower? shower, ITreeItem? parent) : base(diskItemSource, shower, parent) { }
        #endregion

        #region [IItemSelected Implementation]
        bool? IItemSelected.Selected { get => Selected; set => Selected = value; }
        bool IItemSelected.IsExported { get => IsExported; set => IsExported = value; }
        #endregion

        #region [Properties]
        public T2 ViewModel
        {
            get => viewModel;
        }

        public bool IsExported
        {
            get => viewModel.IsExported;
            set
            {
                ViewModel.IsExported = value;
                ViewModel.SetParentExported(Parent, value);
            }
        }

        public virtual bool? Selected
        {
            get
            {
                return ViewModel.IsSelected;
            }

            set
            {
                ViewModel.IsSelected = value;
                ViewModel.SetParentSelected(Parent, value);
            }
        }
        #endregion

        #region [Methods]
        protected override void InitSourceItems()
        {
            base.InitSourceItems();
            viewModel = new T2();
            ViewModel.SetSelecetdFromParent(Parent);
            ViewModel.SetExportedFromParent(Parent);
        }

        public override async Task ExportItemAsync(IExport exporter)
        {
            await base.ExportItemAsync(exporter).ContinueWith((t) =>
            {
                IsExported = t.IsCompleted;
            });
        }
        #endregion
    }
}
