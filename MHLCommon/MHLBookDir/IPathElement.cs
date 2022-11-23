namespace MHLCommon.MHLBookDir
{
    public interface IPathElement
    {
        bool IsTyped { get; }
        string Name { get; }
        BookPathItem ElementType { get; }
    }

    public interface IPathElementTyped<T> : IPathElement
    {
        T TypedItem { get; set; }
    }
}