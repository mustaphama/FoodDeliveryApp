namespace FDA.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public int? DeliveryGuyId { get; set; }
        public int? PromotionCardId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual DeliveryGuy DeliveryGuy { get; set; }
        public virtual PromotionCard PromotionCard { get; set; }
    }

}
