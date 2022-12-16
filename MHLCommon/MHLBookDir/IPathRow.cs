using System.Collections.ObjectModel;

namespace MHLCommon.MHLBookDir
{
    public interface IPathRow<out T> where T : IPathRowElement
    {
        bool IsFileName { get; set; }
        IEnumerable<IPathRow<T>> SubRows {get; }
        IEnumerable<T> Items { get; }
        public int Count { get; }
        public void InsertTo(int i);
        public void RemoveFrom(int i);
        T this[int i] { get; }
    }
    public interface IPathRowUI<out T>: IPathRow<T> where T : IPathRowElement
    {
        bool IsExpanded { get; set; }
    }
}
