namespace FDA.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class UpdateAddressRequest
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
    public class UpdatePhoneRequest
    {
        public string Phone { get; set; }
    }
}
