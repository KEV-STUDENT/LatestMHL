using System.Xml;

namespace MHLCommon.MHLBook
{
    public interface IBook
    {
        string Title { get; }
        List<IBookAttribute<XmlNode>> Authors { get; }
        List<IBookAttribute<XmlNode>> Genres { get; }
        List<IBookAttribute<string>> Keywords { get; }
        string Annotation { get; }
        string Cover { get; }
    }
}