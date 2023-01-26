namespace MHLCommon.DataModels
{
    public interface IBook<T1,T2, T3>
    {
        string Title { get; set; }
        List<T1> Authors { get; set; }       
        List<T2> Genres { get; set; }
        List<T3> Keywords { get; set; }
        string Annotation { get; set; }
        string Cover { get; set; }
    }   
}