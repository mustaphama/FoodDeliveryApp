using Microsoft.EntityFrameworkCore;
using FDA.Models;
namespace FDA.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<UserCards> UserCards { get; set; }
        public DbSet<DeliveryGuy> DeliveryGuys { get; set; }
        public DbSet<PromotionCard> PromotionCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Users table
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // Configure Restaurants table
            modelBuilder.Entity<Restaurant>()
                .HasKey(r => r.Id);

            // Configure Menus table
            modelBuilder.Entity<Menu>()
                .HasKey(m => m.Id);
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Restaurant)
                .WithMany() // One-to-many relation, restaurant can have multiple menus
                .HasForeignKey(m => m.Id_Restaurants)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed

            // Configure Categories table
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);

            // Configure FoodItems table
            modelBuilder.Entity<FoodItem>()
                .HasKey(f => f.Id);
            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.Category)
                .WithMany() // One-to-many relation
                .HasForeignKey(f => f.Id_Categories)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed
            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.Menu)
                .WithMany() // One-to-many relation
                .HasForeignKey(f => f.Id_Menus)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed

            // Configure PromotionCards table
            modelBuilder.Entity<PromotionCard>()
                .HasKey(pc => pc.Id);

            // Configure UserCards table
            modelBuilder.Entity<UserCards>()
                .HasKey(uc => uc.Id);
            modelBuilder.Entity<UserCards>()
                .HasOne(uc => uc.User)
                .WithMany() // One-to-many relation
                .HasForeignKey(uc => uc.Id_Users)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed
            modelBuilder.Entity<UserCards>()
                .HasOne(uc => uc.PromotionCard)
                .WithMany() // One-to-many relation
                .HasForeignKey(uc => uc.Id_PromotionCards)
                .OnDelete(DeleteBehavior.SetNull); // Adjust delete behavior as needed

            // Configure DeliveryGuys table
            modelBuilder.Entity<DeliveryGuy>()
                .HasKey(dg => dg.Id);

            // Configure Orders table
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany() // One-to-many relation
                .HasForeignKey(o => o.Id_Users)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed
            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeliveryGuy)
                .WithMany() // One-to-many relation
                .HasForeignKey(o => o.Id_DeliveryGuys)
                .OnDelete(DeleteBehavior.SetNull); // Adjust delete behavior as needed
            modelBuilder.Entity<Order>()
                .HasOne(o => o.PromotionCard)
                .WithMany() // One-to-many relation
                .HasForeignKey(o => o.Id_PromotionCards)
                .OnDelete(DeleteBehavior.SetNull); // Adjust delete behavior as needed
                                                   // Ignore the Restaurant navigation property for now
            modelBuilder.Entity<Order>()
                .Ignore(o => o.Restaurant);
            // Configure OrderItems table
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.Id);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.FoodItem)
                .WithMany() // One-to-many relation
                .HasForeignKey(oi => oi.Id_FoodItems)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany() // One-to-many relation
                .HasForeignKey(oi => oi.Id_Orders)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed
        }


    }
}
