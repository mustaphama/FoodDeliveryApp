namespace FDA.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public int Id_Restaurants { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public virtual Restaurant Restaurant { get; set; }
    }

}
