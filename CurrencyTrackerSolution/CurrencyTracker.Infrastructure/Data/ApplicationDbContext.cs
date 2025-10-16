using CurrencyTracker.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        private readonly IConfiguration _configuration;

        public DbSet<Valute> Valutes { get; set; }

        public DbSet<Currency> Currencies { get; set; }

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
            });
        }
    }
}
