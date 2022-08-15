namespace MHLCommon
{
    public interface IDiskItemVirtualGroup : IDiskItem, IDiskCollection
    {

        IDiskCollection ParentCollection { get; }
        List<string> ItemsNames { get; }
    }
}