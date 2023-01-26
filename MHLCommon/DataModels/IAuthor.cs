using MHLCommon.MHLBook;
using System.Xml;

namespace MHLCommon.DataModels
{
    public interface IAuthor
    {
        string? FirstName { get; set; }
        string? MiddleName { get; set; }
        string? LastName { get; set; }
    }
}