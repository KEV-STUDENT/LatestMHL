namespace MHLCommon
{
    public interface IDiskCollection
    {
        const int MaxItemsInVirtualGroup = 100;
        int Count { get; }
        bool IsVirtualGroupsUsed { get; }
        IEnumerable<IDiskItem> GetChilds();
        IEnumerable<IDiskItem> GetChilds(List<string> subList);
    }
}