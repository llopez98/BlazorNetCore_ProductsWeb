﻿using BLazorProducts.Server.Context.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BLazorProducts.Server.Context
{
    public class ProductContext : IdentityDbContext<User>
    {
        public ProductContext(DbContextOptions options) : base(options) { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Product> Products { get; set; }
    }
}
