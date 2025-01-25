namespace FDA.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int Id_Orders { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }

        // Navigation Property
        public virtual Order Order { get; set; }
    }

}
