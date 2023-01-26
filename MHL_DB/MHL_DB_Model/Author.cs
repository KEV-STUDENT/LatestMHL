using MHLCommon.DataModels;

namespace MHL_DB_Model
{
    public interface IAuthorDB : IAuthor
    {
        int Id { get; set; }
        List<Book> Books { get; set;}
    }

    public class Author : IAuthorDB
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public List<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }

        int IAuthorDB.Id { get => Id; set => Id = value; }
        string? IAuthor.FirstName { get => FirstName; set => FirstName = value; }
        string? IAuthor.MiddleName { get => MiddleName; set => MiddleName = value; }
        string? IAuthor.LastName { get => LastName; set => LastName = value; }
        List<Book> IAuthorDB.Books { get => Books; set => Books = value; }
    }
}