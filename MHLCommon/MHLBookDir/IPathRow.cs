using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MHLCommon.MHLBookDir
{
    public interface IPathRow
    {
        public int Count { get; }
        public void InsertTo(int i);
        public void RemoveFrom(int i);
        bool IsExpanded { get; set; }
        bool IsFileName { get; set; }
    }
    public interface IPathRow<T> : IPathRow
    {
        T this[int i] { get; set; }
        ObservableCollection<T> Items { get; set; }
    }
    public interface IPathRow<T,K>: IPathRow<T>
    {
        ObservableCollection<K> SubRows { get; set; }
    }
}
