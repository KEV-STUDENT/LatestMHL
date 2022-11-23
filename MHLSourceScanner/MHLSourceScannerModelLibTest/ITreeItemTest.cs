using MHLSourceScannerModelLib;
using System.Collections.ObjectModel;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;

namespace MHLSourceScannerModelLibTest
{

    public class ViewModelTest : ViewModel, ISelected
    {
        private bool? _selected;
        bool? ISelected.IsSelected { get => _selected; set =>_selected = value; }

        void ISelected.SetParentSelected(ITreeItem? parent, bool? value)
        {
            throw new NotImplementedException();
        }

        void ISelected.SetSelecetdFromParent(ITreeItem? parent)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class ITreeItemTest
    {
        [TestMethod]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-284000-291999.zip")]
        public void LoadChilds_pathDirZip(string pathDirZip)
        {
            TreeItemShower shower = new TreeItemShower();
            ITreeDiskItem treeItem = new TreeDiskItem(pathDirZip, shower, null);
            ((TreeDiskItem)treeItem).TestMode = true;
            treeItem.LoadChilds();
            Assert.AreNotEqual(0, treeItem.SourceItems.Count);
        }

        [TestMethod]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-284000-291999.zip")]
        public void LoadChildsCollection_pathDirZip(string pathDirZip)
        {
            TreeItemShower shower = new TreeItemShower();
            ITreeItemCollection treeItem = new TreeDiskItem(pathDirZip, shower, null);
            ObservableCollection<ITreeItem> collection = treeItem.LoadChildsCollection();
            
            foreach (ITreeItem item in collection)
                System.Diagnostics.Debug.WriteLine(item.Name);

            Assert.AreNotEqual(0, collection.Count);
        }

        [TestMethod]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-284000-291999.zip")]
        public void LoadChildsCollectionAsync_pathDirZip(string pathDirZip)
        {
            TreeItemShower shower = new TreeItemShower();
            ITreeItemCollection treeItem = new TreeDiskItem(pathDirZip, shower, null);
            Task<ObservableCollection<ITreeItem>> task = treeItem.LoadChildsCollectionAsync();
            task.Wait();
            foreach (ITreeItem item in task.Result)
                System.Diagnostics.Debug.WriteLine(item.Name);

            Assert.AreNotEqual(0, task.Result.Count);
        }

        [TestMethod]
        [DataRow(@"E:\librus_MyHomeLib\lib.rus.ec\fb2-284000-291999.zip")]
        public void LoadItemCollection_pathDirZip(string pathDirZip)
        {
            TreeItemShower shower = new TreeItemShower();
            ITreeDiskItem treeItem = new TreeDiskItem(pathDirZip, shower, null);
            ((TreeDiskItem)treeItem).TestMode = true;
            
            treeItem.LoadItemCollection();
            
            foreach (ITreeDiskItem item in treeItem.SourceItems)
                System.Diagnostics.Debug.WriteLine(item.Name);

            Assert.AreNotEqual(0, treeItem.SourceItems.Count);
        }
    }
}
