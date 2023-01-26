using MHLCommon.DataModels;
using MHLCommon;

namespace MHL_DB_Model
{
    public interface IBookDB : IBook<Author,Genre4Book, Keyword4Book>
    {
        int Id { get; set; }
        string? EntityInZIP { get; set; }
        string Path2File { get; set; }

        BookFileExtends Extends { get; set; }
    }

    public class Book : IBookDB
    {
        public int Id { get; set; }
        public string? EntityInZIP { get; set; }
        public string Path2File { get; set; }
        public string Title { get; set; }
        public List<Author> Authors { get; set; }
        public List<Genre4Book> Genres { get; set; }
        public List<Keyword4Book> Keywords { get; set; }
        public string Annotation { get; set; }
        public string Cover { get; set; }
        public BookFileExtends Extends { get; set; }

        public Book()
        {
            Authors = new List<Author>();
        }

        int IBookDB.Id { get => Id; set => Id = value; }
        string? IBookDB.EntityInZIP { get => EntityInZIP; set => EntityInZIP = value; }
        string IBookDB.Path2File { get => Path2File; set => Path2File = value; }
        string IBook<Author, Genre4Book, Keyword4Book>.Title { get => Title; set => Title = value; }
        List<Author> IBook<Author, Genre4Book, Keyword4Book>.Authors { get => Authors; set => Authors = value; }
        string IBook<Author, Genre4Book, Keyword4Book>.Annotation { get => Annotation; set => Annotation = value; }
        string IBook<Author, Genre4Book, Keyword4Book>.Cover { get => Cover; set => Cover = value; }
        BookFileExtends IBookDB.Extends { get => Extends; set => Extends = value; }
        List<Genre4Book> IBook<Author, Genre4Book, Keyword4Book>.Genres { get => Genres; set => Genres = value; }
        List<Keyword4Book> IBook<Author, Genre4Book, Keyword4Book>.Keywords { get => Keywords; set => Keywords = value; }
    }
}