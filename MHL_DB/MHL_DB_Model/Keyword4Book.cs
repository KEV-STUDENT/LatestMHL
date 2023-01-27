using MHLCommon.DataModels;
using System.ComponentModel.DataAnnotations;

namespace MHL_DB_Model
{
    public interface IKeywordDB : IKeyword
    {
        int Id { get; set; }
        List<Book> Books { get; set; }
    }
    public class Keyword4Book : IKeywordDB
    {
        public int Id { get; set; }
        public List<Book> Books { get; set; }
        [Required]
        public string Keyword { get; set; }

        public Keyword4Book()
        {
            Books= new List<Book>();
        }

        int IKeywordDB.Id { get => Id; set => Id = value; }
        List<Book> IKeywordDB.Books { get => Books; set => Books = value; }
        string IKeyword.Keyword { get => Keyword; set => Keyword = value; }
    }
}
