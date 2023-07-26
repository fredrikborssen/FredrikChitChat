namespace ChittyChatty.Domain.Entites
{
    public class BrokerListingHouse
    {
        public Guid BrokerId { get; set; }
        public Guid BuildingId { get; set; }
        public Broker Broker { get; set; }
        public House House { get; set; }


        public BrokerListingHouse()
        {

        }

        public BrokerListingHouse(Guid brokerId, Guid buildId)
        {
            BrokerId = brokerId;
            BuildingId = buildId;
        }
    }
}
