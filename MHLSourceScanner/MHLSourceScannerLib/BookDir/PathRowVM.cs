using MHLCommon.MHLBookDir;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace MHLSourceScannerLib.BookDir
{
    public class PathRowVM : IPathRowUI<PathRowElementVM>
    {
        #region [Fields]       
        private ViewModel4PathRow viewModel;
        private ObservableCollection<PathRowElementVM> items = new ObservableCollection<PathRowElementVM>();
        private bool isFileName = true;
        private bool isExpanded = true;

        private PathRowVM? parent;
        private ObservableCollection<PathRowVM> subRows;
        #endregion

        #region [Constructors]       
        public PathRowVM(PathRowVM? parent)
        {
            InitPathRow(parent);
        }

        public PathRowVM() : this(null) { }

        [JsonConstructor]
        public PathRowVM(ObservableCollection<PathRowElementVM> items, bool isFileName, ObservableCollection<PathRowVM> subRows)
        {
            IsFileName = isFileName;
            Items = items ?? new ObservableCollection<PathRowElementVM>();
            SubRows = subRows ?? new ObservableCollection<PathRowVM>();
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
        public PathRowVM? Parent { 
            get => parent; 
            set => parent = value;
        }
       
        public ObservableCollection<PathRowVM> SubRows
        {
            get => subRows;
            set => subRows = value;
        }
       
        public ObservableCollection<PathRowElementVM> Items
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
        private void InitPathRow(PathRowVM? parent)
        {
            this.parent = parent;
            items.Add(new PathRowElementVM());
            viewModel = new ViewModel4PathRow(this);
            subRows = new ObservableCollection<PathRowVM>();
        }

        public void InsertTo(int i)
        {
            if (i < items.Count)
                items.Insert(i, new PathRowElementVM());
            else
                items.Add(new PathRowElementVM());
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
        public PathRowElementVM this[int i]
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
        bool IPathRow<PathRowElementVM>.IsFileName { get => IsFileName; set => IsFileName = value; }
        IEnumerable<IPathRow<PathRowElementVM>> IPathRow<PathRowElementVM>.SubRows { get => SubRows;}

        int IPathRow<PathRowElementVM>.Count => Count;      
        void IPathRow<PathRowElementVM>.InsertTo(int i)
        {
            InsertTo(i);
        }
        void IPathRow<PathRowElementVM>.RemoveFrom(int i)
        {
            RemoveFrom(i);
        }
        PathRowElementVM IPathRow<PathRowElementVM>.this[int i]{ get => this[i];}
        IEnumerable<PathRowElementVM> IPathRow<PathRowElementVM>.Items { get => Items;}
        bool IPathRowUI<PathRowElementVM>.IsExpanded { get => IsExpanded; set => IsExpanded = value; }
        #endregion
    }
}
