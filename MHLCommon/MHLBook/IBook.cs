using MHLCommon.DataModels;
using System.Xml;

namespace MHLCommon.MHLBook
{
    public interface IMHLBook : IBook<MHLAuthor, MHLGenre, MHLKeyword>
    {
        List<MHLSequenceNum> SequenceAndNumber { get; }
    }
}