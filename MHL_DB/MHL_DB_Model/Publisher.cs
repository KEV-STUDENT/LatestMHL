using MHLCommon.DataModels;

namespace MHL_DB_Model
{
    public interface IPublisherDB : IPublisher
    {
        int Id { get; set; }
        List<Book> Books { get; set; }
    }

    public class Publisher : IPublisherDB
    {
        public int Id { get; set; }
        public List<Book> Books { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public Publisher()
        {
            Name= string.Empty;
            City= string.Empty;
            Books = new List<Book>();
        }

        int IPublisherDB.Id { get => Id; set => Id = value; }
        List<Book> IPublisherDB.Books { get => Books; set => Books = value; }
        string IPublisher.Name { get => Name; set => Name = value; }
        string IPublisher.City { get => City; set => City = value; }
    }
}