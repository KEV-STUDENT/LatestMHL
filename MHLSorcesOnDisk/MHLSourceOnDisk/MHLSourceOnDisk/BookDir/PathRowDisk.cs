using MHLCommon.BookDir;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MHLSourceOnDisk.BookDir
{
    public class PathRowDisk : PathRowWithList<PathRowDisk,PathRowElement>
    {
        public PathRowDisk() : this(null, null)
        {
        }

        public PathRowDisk(PathRowDisk? parent) : base(parent)
        {
        }

        public PathRowDisk(ObservableCollection<PathRowElement>? items) : base(items)
        {
        }

        public PathRowDisk(ObservableCollection<PathRowElement>? items, PathRowDisk? parent) : base(items, parent)
        {
        }
        [JsonConstructor]
        public PathRowDisk(bool isFileName, ObservableCollection<PathRowDisk>? subRows, ObservableCollection<PathRowElement>? items) : base(isFileName, subRows, items)
        {
        }
        public PathRowDisk(bool isFileName, ObservableCollection<PathRowDisk>? subRows, ObservableCollection<PathRowElement>? items, PathRowDisk? parent) : base(isFileName, subRows, items, parent)
        {
        }

        protected override void InitPathRow(PathRowDisk parent)
        {
            Parent = parent;
            Items.Add(new PathRowElement());
        }
    }
}
