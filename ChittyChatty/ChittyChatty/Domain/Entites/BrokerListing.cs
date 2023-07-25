#nullable disable
namespace ChittyChatty.Domain.Entites
{
    public class BrokerListing
    {
        public Guid BrokerId { get; set; }
        public Guid BuildId { get; set; }
        public Broker Broker { get; set; }
        public Apartment Apartment { get; set; }
        public House House { get; set; }
    }
}
