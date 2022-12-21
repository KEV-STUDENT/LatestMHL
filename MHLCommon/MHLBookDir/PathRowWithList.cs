using MHLCommon.MHLBookDir;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MHLCommon.BookDir
{
    public abstract class PathRowWithList<T1,T2> : PathRow<T1>, IPathRow<T1,T2>
        where T1 : IPathRow
        where T2 : IPathRowElement, new()
    {
        #region [Fields]       
        private ObservableCollection<T2> items;
        private bool isFileName = true;
        private bool isExpanded = true;

        private IPathRow? parent;
        private ObservableCollection<T1> subRows;
        #endregion

        #region [Constructors]
        protected PathRowWithList(T1? parent) : this(false, null, null, parent) { }
        protected PathRowWithList(ObservableCollection<T2>? items) : this(false, null, items) { }

        protected PathRowWithList(ObservableCollection<T2>? items, T1? parent) : this(false, null, items, parent) { }
        protected PathRowWithList(bool isFileName, ObservableCollection<T1>? subRows, ObservableCollection<T2>? items) :
             base(isFileName, subRows)
        {
            this.items = items ?? new ObservableCollection<T2>();
        }
        protected PathRowWithList(bool isFileName, ObservableCollection<T1>? subRows, ObservableCollection<T2>? items, T1? parent) :
            base(isFileName, subRows)
        {
            this.items = items ?? new ObservableCollection<T2>();
            InitPathRow(parent);
        }
        #endregion

        #region [Properties]       
        [JsonIgnore]
        public int Count => Items.Count;

        [JsonIgnore]
        public bool IsExpanded
        {
            get => isExpanded;
            set => isExpanded = value;
        }

        [JsonIgnore]
        public override IPathRow Parent
        {
            get => parent;
            set => parent = value;
        }

        public override ObservableCollection<T1> SubRows
        {
            get => subRows;
            set => subRows = value;
        }

        public ObservableCollection<T2> Items
        {
            get => items;
            set => items = value;
        }
        public override bool IsFileName
        {
            get => isFileName;
            set => isFileName = value;
        }
        #endregion

        #region [Methods]
        protected abstract void InitPathRow(T1 parent);

        public void InsertTo(int i)
        {
            if (i < items.Count)
                items.Insert(i, new T2());
            else
                items.Add(new T2());
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
        public T2 this[int i]
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
        int IPathRow<T1,T2>.Count => Count;
        void IPathRow<T1,T2>.InsertTo(int i)
        {
            InsertTo(i);
        }
        void IPathRow<T1, T2>.RemoveFrom(int i)
        {
            RemoveFrom(i);
        }
        T2 IPathRow<T1, T2>.this[int i] { get => this[i]; }
        IEnumerable<T2> IPathRow<T1, T2>.Items { get => Items; }
        #endregion
    }
}