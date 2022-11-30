using MHLCommon.MHLBookDir;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MHLSourceScannerLib.BookDir
{
    public class PathRow : TreeItemCollection, IPathRow<PathRowElement>
    {
        #region [Fields]       
        private ViewModel4PathRow viewModel;
        private ObservableCollection<PathRowElement> items = new ObservableCollection<PathRowElement>();
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
        public ViewModel4PathRow ViewModel => viewModel;
        public ObservableCollection<PathRowElement> Items => items;
        #endregion


        #region [Methods]
        private void InitPathRow()
        {
            items.Add(new PathRowElement());
            viewModel = new ViewModel4PathRow(this);
        }

        private void InsertTo(int i)
        {
            if (i < items.Count)
                items.Insert(i, new PathRowElement());
            else
                items.Add(new PathRowElement());
        }

        private void RemoveFrom(int i)
        {
            if (i < items.Count)
                items.RemoveAt(i);
            else
                items.RemoveAt(items.Count - 1);
        }
        #endregion

        #region [Indexer]
        public PathRowElement this[int i]
        {
            get
            {
                return (i >= items.Count ? items.Last() : items[i]);
            }
            set
            {
                items[i] = value;
            }
        }
        #endregion

        #region [IPathRow Implementation] 
        int IPathRow<PathRowElement>.Count => items.Count;
         ObservableCollection<PathRowElement> IPathRow<PathRowElement>.Items => Items;

        PathRowElement IPathRow<PathRowElement>.this[int i] 
        { get => this[i]; set => this[i] = value; }
       
        void IPathRow<PathRowElement>.InsertTo(int i)
        {
            InsertTo(i);
        }

        void IPathRow<PathRowElement>.RemoveFrom(int i)
        {
            RemoveFrom(i);
        }
        #endregion
    }
}
