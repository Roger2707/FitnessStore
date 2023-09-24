using Fit_API.Models;
using Fit_API.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<ProductInstructor> ProductInstructors { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "Cardio"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Strength"
                }
            );

            builder.Entity<Product>()
                .HasMany(pi => pi.ProductInstructors)
                .WithOne(pi => pi.Product)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Instructor>()
                .HasMany(pi => pi.ProductInstructors)
                .WithOne(pi => pi.Instructor)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed Instructor
            builder.Entity<Instructor>().HasData(
                new Instructor()
                {
                    Id = 1,
                    Name = "Roger",
                },
                new Instructor()
                {
                    Id = 2,
                    Name = "Simon",
                }
            );

            // Seed Product
            builder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    Name = "CaliRide",
                    Description = "A cycling program with several exercies with mix terrain. The intensitive from low to medium and high, so that I consider this program suitable and effiency for all who has the target lose high body fat and improve muscle endurances !",
                    ImageUrl = "test.jpg",
                    Price = 100,
                    CategoryId = 1,
                },
                new Product()
                {
                    Id = 2,
                    Name = "BodyPump",
                    Description = "A weight training program on energetic music. We hope it'll bring to you a precius time for improve muscle endurances and get stronger everyday !",
                    ImageUrl = "test.jpg",
                    Price = 150,
                    CategoryId = 2,
                }
            );

            // Seed ProductInstructor
            builder.Entity<ProductInstructor>().HasData(
                new ProductInstructor()
                {
                    Id = 1,
                    InstructorId = 1,
                    ProductId = 1,
                },
                new ProductInstructor()
                {
                    Id = 2,
                    InstructorId = 1,
                    ProductId = 2,
                },
                new ProductInstructor()
                {
                    Id = 3,
                    InstructorId = 2,
                    ProductId = 1,
                }
            );

            // Seed Role
            builder.Entity<Role>()
                .HasData(
                    new Role { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                    new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN" }
                );


            // Order
            builder.Entity<User>()
                .HasOne(o => o.Address)
                .WithOne()
                .HasForeignKey<UserAddress>(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
