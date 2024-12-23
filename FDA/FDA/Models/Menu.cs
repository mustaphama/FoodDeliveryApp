namespace FDA.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string MenuName { get; set; }
        public string MenuImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public virtual Restaurant Restaurant { get; set; }
    }

}
