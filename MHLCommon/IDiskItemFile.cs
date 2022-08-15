namespace MHLCommon
{
    public interface IDiskItemFile : IDiskItem
    {
        IDiskCollection? Parent { get; }
    }
}