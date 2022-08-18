using System.Xml;

namespace MHLCommon.MHLBook
{
    public interface IBookAttribute<T> where T : class
    {
        T? Node { get; }
    }
}