using System.Text.Json.Serialization;

namespace ChittyChatty.MockData
{
    public class ApartmentData
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("Location")]
        public string? Location { get; set; }
        [JsonPropertyName("Rooms")]
        public int Rooms { get; set; }
        [JsonPropertyName("Size")]
        public int Size { get; set; }
        [JsonPropertyName("Published")]
        public DateTime Published { get; set; }
        [JsonPropertyName("Publisher")]
        public string? Publisher { get; set; }
    }
}
