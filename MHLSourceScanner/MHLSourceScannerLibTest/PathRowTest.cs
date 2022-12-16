using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHLCommon.MHLBookDir;
using MHLCommon.MHLScanner;
using MHLSourceScannerLib.BookDir;

namespace MHLSourceScannerModelLibTest
{
    [TestClass]
    public class PathRowTest
    {
        [TestMethod]
        public void PathRowTest_ITreeItem()
        {
            PathRow pathRow = new PathRow();
            Assert.IsInstanceOfType(pathRow, typeof(IPathRow));
        }
        
        [TestMethod]
        [DataRow(0)]
        public void PathRowTest_I_IPathElement(int i)
        {
            PathRow pathRow = new PathRow();
            //System.Diagnostics.Debug.WriteLine(pathRow[i].Name);
            Assert.IsInstanceOfType(pathRow[i], typeof(PathRowElement));
        }

        [TestMethod]
        public void PathRowTest_InsertTo()
        {
            IPathRow pathRow = new PathRow();
            pathRow.InsertTo(0);
            Assert.AreEqual(2, pathRow.Count);
        }

        [TestMethod]
        public void PathRowTest_RemoveFrom()
        {
            IPathRow pathRow = new PathRow();
            pathRow.InsertTo(0);
            pathRow.InsertTo(0);
            pathRow.RemoveFrom(1);
            Assert.AreEqual(2, pathRow.Count);
        }
    }
}
