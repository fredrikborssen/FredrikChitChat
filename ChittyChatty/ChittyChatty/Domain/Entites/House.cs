using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChittyChatty.Domain.Entites
{
    public class House
    {
        [Key]
        [JsonPropertyName("id")]
        public Guid BuildingId { get; set; }
        public int BrokerId { get; set; }
        [JsonPropertyName("Location")]
        public string? Location { get; set; }
        [JsonPropertyName("Rooms")]
        public int Rooms { get; set; }
        [JsonPropertyName("Size")]
        public int Size { get; set; }
        [JsonPropertyName("Published")]
        public DateTime? Published { get; set; }
        [JsonPropertyName("Publisher")]
        public string? BrokerCompany { get; set; }


        public House(int brokerId,string location, int rooms, int size, DateTime? published, string brokerCompany)
        {
            BuildingId = Guid.NewGuid();
            BrokerId = brokerId;
            Location = location;
            Rooms = rooms;
            Size = size;
            Published = published;
            BrokerCompany = brokerCompany;
        }

        public House() { }
    }
}
