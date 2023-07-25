﻿using System.Drawing;
using System.Text.Json.Serialization;

namespace ChittyChatty.Domain.Entites
{
    public class Apartment
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


        public Apartment(string location, int rooms, int size, DateTime published, string publisher)
        {
            Id = Guid.NewGuid();
            Location = location;
            Rooms = rooms;
            Size = size;
            Published = published;
            Publisher = publisher;
        }
    }
}
