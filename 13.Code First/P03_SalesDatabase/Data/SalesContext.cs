using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions<SalesContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=SalesDb;Integrated Security=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);

                entity.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

                entity.Property(p => p.Price)
               .IsRequired(true);

                entity.Property(p => p.Description)
                .HasMaxLength(250)
                .IsRequired(false)
                .IsUnicode(true)
                .HasDefaultValue("No description");
            });


            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(v => v.CustomerId);

                entity.Property(v => v.Email)
                .HasMaxLength(80)
                .IsRequired(true)
                .IsUnicode(false);

                entity.Property(v => v.Name)
                .HasMaxLength(100)
                .IsRequired(true)
                .IsUnicode(true);

                entity.Property(c => c.CreditCardNumber)
                .IsRequired(true)
                .IsUnicode(false);

            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(d => d.StoreId);

                entity.Property(d => d.Name)
                .HasMaxLength(80)
                .IsRequired(true)
                .IsUnicode(true);

            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(m => m.SaleId);

                entity.Property(m => m.Date)
                .IsRequired(true)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETDATE()");

                entity
               .HasOne(v => v.Customer)
               .WithMany(p => p.Sales)
               .HasForeignKey(v => v.CustomerId);

                entity
                .HasOne(v => v.Product)
                .WithMany(d => d.Sales)
                .HasForeignKey(v => v.ProductId);

                 entity
                .HasOne(v => v.Store)
                .WithMany(d => d.Sales)
                .HasForeignKey(v => v.StoreId);
            });
        }
    }
}
