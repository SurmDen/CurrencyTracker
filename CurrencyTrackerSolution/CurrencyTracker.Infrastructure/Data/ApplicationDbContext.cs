using CurrencyTracker.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTracker.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Valute> Valutes { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Valute>(valuteOptions =>
            {
                valuteOptions.HasKey(x => x.Id);
                valuteOptions.HasAlternateKey(x => x.ValuteName);
                valuteOptions.HasIndex(x => x.ValuteName);
                valuteOptions.ToTable("valutes");
            });

            modelBuilder.Entity<Currency>(currencyOptions =>
            {
                currencyOptions.HasKey(x => x.Id);
                currencyOptions.HasIndex(x => x.Date);
                currencyOptions.ToTable("currencies");
                currencyOptions
                    .HasOne(x => x.Valute)
                    .WithMany(x => x.Currencies)
                    .HasForeignKey(x => x.ValuteId)
                    .OnDelete(DeleteBehavior.Cascade);
                currencyOptions
                    .Property(x => x.Date)
                    .HasColumnType("date");
            });

            modelBuilder.Entity<User>(userOptions =>
            {
                userOptions.HasKey(x => x.Id);
                userOptions.HasAlternateKey(x => x.Email);
                userOptions.HasIndex(u => u.Email);
                userOptions.ToTable("users");
                userOptions.HasData(new User()
                {
                    Id = 1,
                    FirstName = "Jeffry",
                    LastName = "Rihter",
                    Email = "user@gmail.com",
                    Password = "123456"
                });
            });
        }
    }
}
