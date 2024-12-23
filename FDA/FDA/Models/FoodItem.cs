namespace FDA.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public virtual Menu Menu { get; set; }
        public virtual Category Category { get; set; }
    }
    public class FoodItemWithRestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int RestaurantId { get; set; } // New property
        public string RestaurantName { get; set; }
        public string RestaurantLocation { get; set; }
    }


}
