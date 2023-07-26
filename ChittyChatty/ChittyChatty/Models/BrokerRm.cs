namespace ChittyChatty.Models
{
    public class BrokerRm
    {
        public Guid BrokerId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string BrokerCompany { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
