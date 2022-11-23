using MHLCommon.MHLBookDir;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System;
using System.Collections.Generic;

namespace MHLSourceScannerLib.BookDir
{
    public class PathRow : TreeItemCollection, IPathRow
    {
        #region [Fields]
        private List<IPathElement> items = new List<IPathElement>();
        private ViewModel4PathRowItem viewModelmodel;
        #endregion

        #region [Constructors]
        public PathRow(string name, ITreeItem? parent) : base(name, parent)
        {
            InitPathRow();
        }

        public PathRow(ITreeItem? parent) : base(parent)
        {
            InitPathRow();
        }

        public PathRow() : base()
        {
            InitPathRow();
        }
        #endregion

        #region [Properties]
        public ViewModel4PathRowItem ViewModel => viewModelmodel;
        #endregion

        #region [Indexer]
        public IPathElement? this[int i]
        {
            get{
                return (i >= items.Count ? null : items[i]);
            }
        }
        #endregion

        #region [Methods]
        private void InitPathRow()
        {
            items.Add(new FirstLetter());
            viewModelmodel= new ViewModel4PathRowItem();

        }
        #endregion

        #region [IPathRow Implementation] 
        int IPathRow.Count => items.Count;

        IPathElement? IPathRow.this[int i] => this[i];
       
        void IPathRow.InsertTo(int i)
        {
            if (i < items.Count)
                items.Insert(i, new PathElement(BookPathItem.Author));
            else
                items.Add(new PathElement(BookPathItem.Author));
        }

        void IPathRow.RemoveFrom(int i)
        {
            if (i < items.Count)
                items.Remove(items[i]);
        }
        #endregion
    }
}
