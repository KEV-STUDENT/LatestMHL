namespace MHLCommon.MHLDiskItems
{
    public interface IVirtualGroup : IDiskCollection
    {

        IDiskCollection ParentCollection { get; }
        List<string> ItemsNames { get; }
    }
}