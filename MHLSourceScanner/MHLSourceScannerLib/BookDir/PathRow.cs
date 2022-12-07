using MHLCommon.MHLBookDir;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace MHLSourceScannerLib.BookDir
{
    public class PathRow : IPathRow<PathRowElement, PathRow>
    {
        #region [Fields]       
        private ViewModel4PathRow viewModel;
        private ObservableCollection<PathRowElement> items = new ObservableCollection<PathRowElement>();
        private bool isFileName = true;
        private bool isExpanded = true;

        private PathRow? parent;
        private ObservableCollection<PathRow> subRows;
        #endregion

        #region [Constructors]       
        public PathRow(PathRow? parent)
        {
            InitPathRow(parent);
        }

        public PathRow() : this(null) { }

        public PathRow(ObservableCollection<PathRowElement> items, bool isFileName, ObservableCollection<PathRow> subRows)
        {
            IsFileName = isFileName;
            Items = items;
            SubRows = subRows;
            viewModel = new ViewModel4PathRow(this);           
        }

        #endregion

        #region [Properties]       
        [JsonIgnore]
        public int Count => Items.Count;

        [JsonIgnore]
        public ViewModel4PathRow ViewModel => viewModel;
        [JsonIgnore]
        public bool IsExpanded
        {
            get => isExpanded;
            set => isExpanded = value;
        }

        [JsonIgnore]
        public PathRow? Parent { 
            get => parent; 
            set => parent = value;
        }
        public ObservableCollection<PathRow> SubRows
        {
            get => subRows;
            set => subRows = value;
        }
        public ObservableCollection<PathRowElement> Items
        {
            get => items;
            set => items = value;
        }
        public bool IsFileName
        {
            get => isFileName;
            set => isFileName = value;
        }
        #endregion


        #region [Methods]
        private void InitPathRow(PathRow? parent)
        {
            this.parent = parent;
            items.Add(new PathRowElement());
            viewModel = new ViewModel4PathRow(this);
            subRows = new ObservableCollection<PathRow>();
        }

        public void InsertTo(int i)
        {
            if (i < items.Count)
                items.Insert(i, new PathRowElement());
            else
                items.Add(new PathRowElement());
        }

        public void RemoveFrom(int i)
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
        int IPathRow.Count => Count;
        bool IPathRow.IsExpanded { get => IsExpanded; set => IsExpanded = value; }
        bool IPathRow.IsFileName { get => IsFileName; set => IsFileName = value; }
        void IPathRow.InsertTo(int i)
        {
            InsertTo(i);
        }

        void IPathRow.RemoveFrom(int i)
        {
            RemoveFrom(i);
        }
        ObservableCollection<PathRow> IPathRow<PathRowElement, PathRow>.SubRows { get => SubRows; set => SubRows = value; }
        PathRowElement IPathRow<PathRowElement>.this[int i]{ get => this[i]; set => this[i] = value; }
        ObservableCollection<PathRowElement> IPathRow<PathRowElement>.Items { get => Items; set => Items = value; }
        #endregion
    }
}
