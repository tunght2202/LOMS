using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;

namespace LOMSAPI.Data.Entities
{
    public class LOMSDbContext : IdentityDbContext<User>
    {
        public LOMSDbContext(DbContextOptions<LOMSDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LiveStream> LiveStreams { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ListProduct> ListProducts { get; set; }
        public DbSet<LiveStreamCustomer> LiveStreamsCustomers { get; set; }
        public DbSet<ProductListProduct> ProductListProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.Entity<LiveStream>()
               .HasOne(ls => ls.User)
               .WithMany(u => u.LiveStreams)
               .HasForeignKey(ls => ls.UserID)
               .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<ProductListProduct>()
                .HasOne(plp => plp.Product)
                .WithMany(p => p.ProductListProducts)
                .HasForeignKey(plp => plp.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductListProduct>()
                .HasOne(plp => plp.ListProduct)
                .WithMany(lp => lp.ProductListProducts)
                .HasForeignKey(plp => plp.ListProductID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LiveStream>()
                .HasOne(l => l.ListProduct)
                .WithMany(lp => lp.LiveStreams)
                .HasForeignKey(l => l.ListProductID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LiveStreamCustomer>()
                .HasOne(lc => lc.Customer)
                .WithMany(c => c.LiveStreamCustomers)
                .HasForeignKey(lc => lc.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LiveStreamCustomer>()
                .HasOne(lc => lc.LiveStream)
                .WithMany(l => l.LiveStreamCustomers)
                .HasForeignKey(lc => lc.LivestreamID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.Comment)
                .WithOne(c => c.Order) 
                .HasForeignKey<Order>(o => o.CommentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders) 
                .HasForeignKey(o => o.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ListProduct>()
                .HasOne(p => p.User)
                .WithMany(u => u.ListProducts)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.user)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.LiveStreamCustomer)
                .WithMany(lc => lc.Comments)
                .HasForeignKey(c => c.LiveStreamCustomerID)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
