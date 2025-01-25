namespace FDA.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Id_Orders { get; set; }
        public int Id_FoodItems { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual FoodItem FoodItem { get; set; }
    }

}
