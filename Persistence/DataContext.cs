using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        #region DbSet

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchCategory> BranchCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Attachment>(x =>
            {
                x.HasOne(a => a.Product)
                    .WithMany(a => a.Attachments)
                    .HasForeignKey(a => a.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(a => a.Category)
                    .WithMany(a => a.Attachments)
                    .HasForeignKey(a => a.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Branch>(x =>
            {
                x.HasOne(a => a.Restaurant)
                    .WithMany(a => a.Branches)
                    .HasForeignKey(a => a.RestaurantId) 
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(a => a.City)
                    .WithMany(a => a.Branches)
                    .HasForeignKey(a => a.CityId)
                    .OnDelete(DeleteBehavior.Restrict);

                x.HasOne(a => a.Province)
                    .WithMany(a => a.Branches)
                    .HasForeignKey(a => a.ProvinceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BranchCategory>(x =>
            {
                x.HasOne(a => a.Branch)
                    .WithMany(a => a.Categories)
                    .HasForeignKey(a => a.BranchId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(a => a.Category)
                    .WithMany(a => a.Branches)
                    .HasForeignKey(a => a.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Category>(x =>
            {
                x.HasOne(a => a.Parent)
                    .WithMany(a => a.Children)
                    .HasForeignKey(a => a.ParentId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(a => a.Restaurant)
                    .WithMany(a => a.Categories)
                    .HasForeignKey(a => a.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<City>(x =>
            {
                x.HasOne(a => a.Province)
                    .WithMany(a => a.Cities)
                    .HasForeignKey(a => a.ProvinceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Feedback>(x =>
            {
                x.HasOne(a => a.OrderDetail)
                    .WithMany(a => a.Feedbacks)
                    .HasForeignKey(a => a.OrderDetailId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Order>(x =>
            {
                x.HasOne(a => a.Branch)
                    .WithMany(a => a.Orders)
                    .HasForeignKey(a => a.BranchId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<OrderDetail>(x =>
            {
                x.HasOne(a => a.Product)
                    .WithMany(a => a.OrderDetails)
                    .HasForeignKey(a => a.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(a => a.Order)
                    .WithMany(a => a.OrderDetails)
                    .HasForeignKey(a => a.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Product>(x =>
            {
                x.HasOne(a => a.Category)
                    .WithMany(a => a.Products)
                    .HasForeignKey(a => a.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<RestaurantModule>(x =>
            {
                x.HasOne(a => a.Restaurant)
                    .WithMany(a => a.Modules)
                    .HasForeignKey(a => a.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(a => a.Module)
                    .WithMany(a => a.Restaurants)
                    .HasForeignKey(a => a.ModuleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<User>(x =>
            {
                x.HasOne(a => a.Restaurant)
                    .WithMany(a => a.Users)
                    .HasForeignKey(a => a.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(a => a.Branch)
                    .WithMany(a => a.Users)
                    .HasForeignKey(a => a.BranchId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
