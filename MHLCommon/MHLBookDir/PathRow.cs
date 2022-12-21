using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MHLCommon.MHLBookDir
{
    public class PathRow<T> : IPathRow<T> where T :IPathRow
    {
        public PathRow():this(false, null) { }
        public PathRow(bool isFileName) : this(isFileName, null) { }
        public PathRow(ObservableCollection<T>? subRows) : this(false, subRows) { }

        [JsonConstructor]
        public PathRow(bool isFileName, ObservableCollection<T>? subRows) { 
            IsFileName= isFileName;
            SubRows= subRows ?? new ObservableCollection<T>();
            foreach(T row in SubRows)
            {
                row.Parent = this;
            }
        }
        public virtual bool IsFileName { get; set; }
        public virtual ObservableCollection<T> SubRows { get; set;  }
        public virtual IPathRow? Parent { get; set; }
        bool IPathRow.IsFileName { get => IsFileName; set => IsFileName = value; }
        IEnumerable<T> IPathRow<T>.SubRows => SubRows;
        IPathRow? IPathRow.Parent { get => Parent; set => Parent = value; }
    }
}
