using MHLSourceScannerModelLib;
using System.Collections.ObjectModel;
using MHLCommon;

namespace MHLSourceScannerModelLibTest
{
    [TestClass]
    public class ITreeItemTest
    {
        protected readonly string pathDir = @"F:\1\test";
        protected readonly string pathDirZip = @"E:\librus_MyHomeLib\lib.rus.ec\fb2-284000-291999.zip";


        [TestMethod]
        public void LoadChilds_pathDirZip()
        {
            DiskItemShower shower = new DiskItemShower();
            ITreeDiskItem treeItem = new TreeDiskItem(pathDirZip, shower);
            ((TreeDiskItem)treeItem).TestMode = true;
            treeItem.LoadChilds();
            Assert.AreNotEqual(0, treeItem.SourceItems.Count);
        }

        [TestMethod]
        public void LoadChildsCollection_pathDirZip()
        {
            DiskItemShower shower = new DiskItemShower();
            ITreeCollectionItem treeItem = new TreeDiskItem(pathDirZip, shower);
            ObservableCollection<ITreeItem> collection = treeItem.LoadChildsCollection();
            
            foreach (ITreeItem item in collection)
                System.Diagnostics.Debug.WriteLine(item.Name);

            Assert.AreNotEqual(0, collection.Count);
        }

        [TestMethod]
        public void LoadChildsCollectionAsync_pathDirZip()
        {
            DiskItemShower shower = new DiskItemShower();
            ITreeCollectionItem treeItem = new TreeDiskItem(pathDirZip, shower);
            Task<ObservableCollection<ITreeItem>> task = treeItem.LoadChildsCollectionAsync();
            task.Wait();
            foreach (ITreeItem item in task.Result)
                System.Diagnostics.Debug.WriteLine(item.Name);

            Assert.AreNotEqual(0, task.Result.Count);
        }


        [TestMethod]
        public void LoadItemCollection_pathDirZip()
        {
            DiskItemShower shower = new DiskItemShower();
            ITreeDiskItem treeItem = new TreeDiskItem(pathDirZip, shower);
            ((TreeDiskItem)treeItem).TestMode = true;
            
            treeItem.LoadItemCollection();
            
            foreach (ITreeDiskItem item in treeItem.SourceItems)
                System.Diagnostics.Debug.WriteLine(item.Name);

            Assert.AreNotEqual(0, treeItem.SourceItems.Count);
        }
    }
}
