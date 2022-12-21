using MHLCommon.MHLBookDir;
using MHLSourceScannerLib.BookDir;
using MHLSourceOnDisk.BookDir;
using System.Text.Json;

namespace MHLSourceScannerModelLibTest
{
    [TestClass]
    public class PathRowTest
    {
        [TestMethod]
        public void PathRowTest_ITreeItem()
        {
            PathRowVM pathRow = new PathRowVM();
            Assert.IsInstanceOfType(pathRow, typeof(IPathRow));
        }

        [TestMethod]
        [DataRow(0)]
        public void PathRowTest_I_IPathElement(int i)
        {
            PathRowVM pathRow = new PathRowVM();
            //System.Diagnostics.Debug.WriteLine(pathRow[i].Name);
            Assert.IsInstanceOfType(pathRow[i], typeof(PathRowElementVM));
        }

        [TestMethod]
        public void PathRowTest_PathRowVMInsertTo()
        {
            IPathRow<IPathRow,IPathRowElement> pathRow = new PathRowVM();
            pathRow.InsertTo(0);
            Assert.AreEqual(2, pathRow.Count);
        }

        [TestMethod]
        public void PathRowTest_PathRowDiskInsertTo()
        {
            IPathRow<IPathRow,IPathRowElement> pathRow = new PathRowDisk();
            pathRow.InsertTo(0);
            Assert.AreEqual(2, pathRow.Count);
        }

        [TestMethod]
        public void PathRowTest_RemoveFrom()
        {
            IPathRow<IPathRow,IPathRowElement> pathRow = new PathRowVM();
            pathRow.InsertTo(0);
            pathRow.InsertTo(0);
            pathRow.RemoveFrom(1);
            Assert.AreEqual(2, pathRow.Count);
        }

        [TestMethod]
        [DataRow(@"{""SubRows"":[{""SubRows"":[{""SubRows"":[],""Items"":[{""SelectedItemType"":2,""SelectedTypedItemType"":0},{""SelectedItemType"":3,""SelectedTypedItemType"":0},{ ""SelectedItemType"":1,""SelectedTypedItemType"":1}],""IsFileName"":true}],""Items"":[{""SelectedItemType"":2,""SelectedTypedItemType"":0},{ ""SelectedItemType"":3,""SelectedTypedItemType"":0}],""IsFileName"":false}],""Items"":[{""SelectedItemType"":2,""SelectedTypedItemType"":0},{ ""SelectedItemType"":3,""SelectedTypedItemType"":0},{ ""SelectedItemType"":1,""SelectedTypedItemType"":3}],""IsFileName"":false}
")]     
        
        public void PathRowTest_Desrialize(string jsonDir) 
        {
            PathRowDisk? row = JsonSerializer.Deserialize<PathRowDisk>(jsonDir);
            System.Diagnostics.Debug.WriteLine(row.Count);
            Assert.IsInstanceOfType(row, typeof(IPathRow));
        }


        [TestMethod]
        [DataRow(@"{""SubRows"":[{""SubRows"":[{""SubRows"":[],""Items"":[{""SelectedItemType"":2,""SelectedTypedItemType"":0},{""SelectedItemType"":3,""SelectedTypedItemType"":0},{ ""SelectedItemType"":1,""SelectedTypedItemType"":1}],""IsFileName"":true}],""Items"":[{""SelectedItemType"":2,""SelectedTypedItemType"":0},{ ""SelectedItemType"":3,""SelectedTypedItemType"":0}],""IsFileName"":false}],""Items"":[{""SelectedItemType"":2,""SelectedTypedItemType"":0},{ ""SelectedItemType"":3,""SelectedTypedItemType"":0},{ ""SelectedItemType"":1,""SelectedTypedItemType"":3}],""IsFileName"":false}
")]

        public void PathRowTest_Row_equal_SubRowParent(string jsonDir)
        {
            PathRowDisk? row = JsonSerializer.Deserialize<PathRowDisk>(jsonDir);
            System.Diagnostics.Debug.WriteLine(row.SubRows[0].Parent == null);
            Assert.AreSame(row, row.SubRows[0].Parent);
        }
    }
}
