using MHLCommon;
using MHLCommon.DataModels;
using System.ComponentModel.DataAnnotations;

namespace MHL_DB_Model
{
    public interface IGenreDB : IGenre
    {
        int Id { get; set; }
        List<Book> Books { get; set; }
        string GenreCode { get; set; }
    }
    public class Genre : IGenreDB
    {
        public int Id { get; set; }
        public List<Book> Books { get; set; }
        [Required]
        public FB2Genres GenreVal { get; set; }
        [Required]
        public string GenreCode { get; set; }

        public Genre()
        {
            Books = new List<Book>();
            GenreCode = GenreVal.ToString();
        }

        int IGenreDB.Id { get => Id; set => Id = value; }
        List<Book> IGenreDB.Books { get => Books; set => Books = value; }
        FB2Genres IGenre.Genre { get => GenreVal; set => GenreVal = value; }
        string IGenreDB.GenreCode { get => GenreCode; set => GenreCode = value; }       
    }
}