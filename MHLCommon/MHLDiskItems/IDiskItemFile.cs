namespace MHLCommon.MHLDiskItems
{
    public interface IDiskItemFile : IDiskItem
    {
        IDiskCollection? Parent { get; }
    }
}