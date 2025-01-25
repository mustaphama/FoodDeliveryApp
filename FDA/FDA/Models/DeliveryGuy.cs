namespace FDA.Models
{
    public class DeliveryGuy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Availability { get; set; } = true;
        public string City { get; set; }
        public string Country { get; set; }
        public string VehiculeType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
