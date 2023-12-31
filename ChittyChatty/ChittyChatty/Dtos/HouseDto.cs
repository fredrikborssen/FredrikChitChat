﻿namespace ChittyChatty.Dtos
{
    public class HouseDto
    {
        public Guid BrokerId { get; set; }
        public string? Location { get; set; }
        public int Rooms { get; set; }
        public int Size { get; set; }
        public DateTime? Published { get; set; }
        public string? BrokerCompany { get; set; }
    }
}
