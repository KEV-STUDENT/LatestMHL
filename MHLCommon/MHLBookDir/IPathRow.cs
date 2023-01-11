namespace MHLCommon.MHLBookDir
{

    public interface IPathRow
    {
        IPathRow? Parent { get; set; }
        bool IsFileName { get; set; }
    }

    public interface IPathRow<out T> : IPathRow where T : IPathRow
    {
        IEnumerable<T> SubRows { get; }
    }

    public interface IPathRow<out T1, out T2>: IPathRow<T1>
        where T1 : IPathRow
        where T2 : IPathRowElement
    {
        IEnumerable<T2> Items { get; }
        T2 this[int i] { get; }
        public int Count { get; }
        public void InsertTo(int i);
        public void RemoveFrom(int i);
    }
    public interface IPathRowUI<out T1, out T2>: IPathRow<T1,T2>
        where T1 : IPathRow
        where T2 : IPathRowElement
    {
        bool IsExpanded { get; set; }
    }
}
