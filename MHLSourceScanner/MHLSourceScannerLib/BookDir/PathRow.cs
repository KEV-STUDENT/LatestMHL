using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace MHLSourceScannerLib.BookDir
{
    public class PathRow : TreeItemCollection
    {
        private const int _cnt = 5;
        #region [Fields]
        private ITreeItem?[] items = new ITreeItem?[_cnt] { new TreeItem("First"), null, null, null, null };
        #endregion

        #region [Constructors]
        public PathRow(string name, ITreeItem? parent) : base(name, parent)
        {
        }

        public PathRow(ITreeItem? parent) : base(parent)
        {
        }

        public PathRow() : base()
        {
        }
        #endregion

        #region [Indexer]
        public ITreeItem? this[int i]
        {
            get{

                return (i > _cnt ? null : items[i]);
            }
        }
        #endregion
    }
}
