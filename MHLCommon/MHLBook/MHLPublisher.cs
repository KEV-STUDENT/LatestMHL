using MHLCommon.DataModels;

namespace MHLCommon.MHLBook
{
    public class MHLPublisher : IPublisher
    {
        public MHLPublisher(string name, string city) { Name = name; City = city; }
        public string Name { get; private set; }
        public string City { get; private set; }
        string IPublisher.Name { get => Name; set => Name = value; }
        string IPublisher.City { get => City; set => City = value; }
    }
}