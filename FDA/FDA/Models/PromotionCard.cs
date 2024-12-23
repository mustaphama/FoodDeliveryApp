namespace FDA.Models
{
    public class PromotionCard
    {
        public int Id { get; set; }
        public string CardCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime? ExpiryDate { get; set; }
    }

}
