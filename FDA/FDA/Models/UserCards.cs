namespace FDA.Models
{
    public class UserCards
    {
        public int Id { get; set; }
        public bool IsUsed { get; set; }
        public int Id_Users { get; set; }
        public int Id_PromotionCards { get; set; }

        public virtual User User { get; set; }
        public virtual PromotionCard PromotionCard { get; set; }
    }
}
