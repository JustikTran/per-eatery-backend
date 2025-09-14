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
            modelBuilder.Entity<VerifyCode>(entity =>
            {
                entity.Property(e => e.Expired)
                      .HasColumnType("TIMESTAMP WITH TIME ZONE")
                      .HasDefaultValueSql("NOW() + interval '5 minutes'");
            });

            modelBuilder.Entity<Dish>().ToTable("Dishes");
        }
    }
}
