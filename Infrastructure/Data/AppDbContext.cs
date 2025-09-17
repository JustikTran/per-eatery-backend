using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<VerifyCode> VerifyCodes { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Messages> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Profile>().ToTable("Profiles");
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.Id);

            modelBuilder.Entity<VerifyCode>().ToTable("VerifyCodes");
            modelBuilder.Entity<VerifyCode>()
                .HasOne(vc => vc.User)
                .WithMany(u => u.VerifyCodes)
                .HasForeignKey(vc => vc.UserId);
            modelBuilder.Entity<VerifyCode>(entity =>
            {
                entity.Property(e => e.Expired)
                      .HasColumnType("TIMESTAMP WITH TIME ZONE")
                      .HasDefaultValueSql("NOW() + interval '5 minutes'");
            });

            modelBuilder.Entity<Dish>().ToTable("Dishes");

            modelBuilder.Entity<Cart>().ToTable("Carts");
            modelBuilder.Entity<Cart>()
                .HasOne(u => u.User)
                .WithOne(c => c.Cart)
                .HasForeignKey<Cart>(c => c.UserId);
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>().ToTable("CartItems");
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Dish)
                .WithMany(d => d.CartItems)
                .HasForeignKey(ci => ci.DishId);

            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethods");
            modelBuilder.Entity<PaymentMethod>()
                .HasMany(pm => pm.Orders)
                .WithOne(o => o.PaymentMethod)
                .HasForeignKey(o => o.PaymentMethodId);

            modelBuilder.Entity<Messages>().ToTable("Messages");
            modelBuilder.Entity<Messages>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId);
        }
    }
}
