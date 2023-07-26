#nullable disable
namespace ChittyChatty.Domain.Entites
{
    public class BrokerListingApartment
    {
        public Guid BrokerId { get; set; }
        public Guid BuildingId { get; set; }
        public Broker Broker { get; set; }
        public Apartment Apartment { get; set; }


        public BrokerListingApartment()
        {
            
        }

        public BrokerListingApartment(Guid brokerId, Guid buildId)
        {
            BrokerId = brokerId;
            BuildingId = buildId;
        }
    }
}
