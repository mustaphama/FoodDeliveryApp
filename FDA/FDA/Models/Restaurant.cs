namespace FDA.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LogoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
