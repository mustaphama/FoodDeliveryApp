namespace FDA.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public int Id_Menus { get; set; }
        public int Id_Categories { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public double Ratings { get; set; }
        public int NumOfReviews { get; set; }
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
        public int Id_Restaurants { get; set; } // New property
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
    }
    public class RatingRequest
    {
        public int UserId { get; set; } // The ID of the user submitting the rating
        public int FoodItemId { get; set; } // The ID of the food item being rated
        public int Rating { get; set; } // Rating value (e.g., 1 to 5)
    }


}
