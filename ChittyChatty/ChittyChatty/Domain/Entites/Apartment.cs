using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;

namespace ChittyChatty.Domain.Entites
{
    public class Apartment
    {
        [JsonPropertyName("BuildingId")]
        public Guid BuildingId { get; set; }
        [JsonPropertyName("BrokerId")]
        public Guid BrokerId { get; set; }
        [JsonPropertyName("Location")]
        public string? Location { get; set; }
        [JsonPropertyName("Rooms")]
        public int Rooms { get; set; }
        [JsonPropertyName("Size")]
        public int Size { get; set; }
        [JsonPropertyName("Published")]
        public DateTime? Published { get; set; }
        [JsonPropertyName("BrokerCompany")]
        public string? BrokerCompany { get; set; }


        public Apartment(Guid brokerId,string location, int rooms, int size, DateTime? published, string brokerCompany)
        {
            BuildingId = Guid.NewGuid();
            BrokerId = brokerId;
            Location = location;
            Rooms = rooms;
            Size = size;
            Published = published;
            BrokerCompany = brokerCompany;
        }
    }
}
