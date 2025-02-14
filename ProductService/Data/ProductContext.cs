﻿using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data
{
    public class ProductContext: DbContext

    {
        public ProductContext(DbContextOptions<ProductContext> options): base(options)
{}
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products"); // Assurez-vous que la table est bien nommée "Products"
        }
    }
}
