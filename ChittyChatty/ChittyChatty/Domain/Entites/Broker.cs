#nullable disable
using System.ComponentModel.DataAnnotations;

namespace ChittyChatty.Domain.Entites
{
    public class Broker
    {
        [Key]
        public Guid BrokerId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string BrokerCompany { get; set; }
        public DateTime LastUpdate { get; set; }

        public Broker()
        {
            
        }

        public Broker(string firstName, string surname, string brokerCompany, DateTime lastUpdate)
        {
            BrokerId = Guid.NewGuid();
            FirstName = firstName;
            Surname = surname;
            BrokerCompany = brokerCompany;
            LastUpdate = lastUpdate;
        }
    }
}
