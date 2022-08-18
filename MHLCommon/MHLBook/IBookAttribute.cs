using System.Xml;

namespace MHLCommon.MHLBook
{
    public interface IBookAttribute
    {
        XmlNode? Node { get; }
    }
}