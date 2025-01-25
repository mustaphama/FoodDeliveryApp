namespace FDA.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int Id_Users { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public int? Id_DeliveryGuys { get; set; }
        public int? Id_PromotionCards { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual DeliveryGuy DeliveryGuy { get; set; }
        public virtual PromotionCard PromotionCard { get; set; }
    }
    public class CartRequest
    {
        public int Id_Users { get; set; } // Retrieve this from Preferences
        public List<CartItem> CartItems { get; set; }
    }

    public class CartItem
    {
        public int Id_FoodItems { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
