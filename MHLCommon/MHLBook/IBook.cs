using MHLCommon.DataModels;

namespace MHLCommon.MHLBook
{
    public interface IMHLBook : IBook<MHLAuthor, MHLGenre, MHLKeyword>
    {
        MHLSequenceNum? SequenceAndNumber { get; }
        IPublisher? Publisher { get; }
        int? Year { get; }
    }
}