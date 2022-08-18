namespace MHLCommon.MHLBook
{
    public interface IBook
    {
        string Title { get; }
        List<IBookAttribute> Authors { get; }
        List<IBookAttribute> Genres { get; }
    }
}