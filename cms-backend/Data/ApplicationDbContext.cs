﻿using cms_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace cms_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@doe.com",
                    CreatedAt = new DateTime(2025, 5, 20, 17, 10, 19, DateTimeKind.Utc)

                },
                new User
                {
                    Id = 2,
                    Name = "Minhazul Islam Sohag",
                    Email = "sohag@email.com",
                    CreatedAt = new DateTime(2025, 5, 20, 17, 10, 19, DateTimeKind.Utc)
                }
            );
        }
    }
}
