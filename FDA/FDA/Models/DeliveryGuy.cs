namespace FDA.Models
{
    public class DeliveryGuy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Availability { get; set; } = true;
        public string VehicleType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
