namespace ChittyChatty.Models
{
    public class HouseRm
    {
        public Guid Id { get; set; }
        public string? Location { get; set; }
        public int Rooms { get; set; }
        public int Size { get; set; }
        public DateTime Published { get; set; }
        public string? Publisher { get; set; }
    }
}
