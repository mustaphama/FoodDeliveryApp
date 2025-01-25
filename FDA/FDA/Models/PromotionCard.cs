namespace FDA.Models
{
    public class PromotionCard
    {
        public int Id { get; set; }
        public string CardCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal MinAmount { get; set; }
        public string DiscountType { get; set; }
    }

}
