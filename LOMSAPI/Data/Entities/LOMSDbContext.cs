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
        public virtual DbSet<AdministrativeRegion> AdministrativeRegions { get; set; }
        public virtual DbSet<AdministrativeUnit> AdministrativeUnits { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

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

            builder.Entity<AdministrativeRegion>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("administrative_regions_pkey");

                entity.ToTable("administrative_regions");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .HasColumnName("code_name");
                entity.Property(e => e.CodeNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("code_name_en");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .HasColumnName("name_en");
            });

            builder.Entity<AdministrativeUnit>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("administrative_units_pkey");

                entity.ToTable("administrative_units");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .HasColumnName("code_name");
                entity.Property(e => e.CodeNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("code_name_en");
                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");
                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("full_name_en");
                entity.Property(e => e.ShortName)
                    .HasMaxLength(255)
                    .HasColumnName("short_name");
                entity.Property(e => e.ShortNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("short_name_en");
            });

         

            builder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.Code).HasName("districts_pkey");

                entity.ToTable("districts");

                entity.HasIndex(e => e.ProvinceCode, "idx_districts_province");

                entity.HasIndex(e => e.AdministrativeUnitId, "idx_districts_unit");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .HasColumnName("code");
                entity.Property(e => e.AdministrativeUnitId).HasColumnName("administrative_unit_id");
                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .HasColumnName("code_name");
                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");
                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("full_name_en");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .HasColumnName("name_en");
                entity.Property(e => e.ProvinceCode)
                    .HasMaxLength(20)
                    .HasColumnName("province_code");

                entity.HasOne(d => d.AdministrativeUnit).WithMany(p => p.Districts)
                    .HasForeignKey(d => d.AdministrativeUnitId)
                    .HasConstraintName("districts_administrative_unit_id_fkey");

                entity.HasOne(d => d.ProvinceCodeNavigation).WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceCode)
                    .HasConstraintName("districts_province_code_fkey");
            });

            builder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.Code).HasName("provinces_pkey");

                entity.ToTable("provinces");

                entity.HasIndex(e => e.AdministrativeRegionId, "idx_provinces_region");

                entity.HasIndex(e => e.AdministrativeUnitId, "idx_provinces_unit");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .HasColumnName("code");
                entity.Property(e => e.AdministrativeRegionId).HasColumnName("administrative_region_id");
                entity.Property(e => e.AdministrativeUnitId).HasColumnName("administrative_unit_id");
                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .HasColumnName("code_name");
                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");
                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("full_name_en");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .HasColumnName("name_en");

                entity.HasOne(d => d.AdministrativeRegion).WithMany(p => p.Provinces)
                    .HasForeignKey(d => d.AdministrativeRegionId)
                    .HasConstraintName("provinces_administrative_region_id_fkey");

                entity.HasOne(d => d.AdministrativeUnit).WithMany(p => p.Provinces)
                    .HasForeignKey(d => d.AdministrativeUnitId)
                    .HasConstraintName("provinces_administrative_unit_id_fkey");
            });

            builder.Entity<Ward>(entity =>
            {
                entity.HasKey(e => e.Code).HasName("wards_pkey");

                entity.ToTable("wards");

                entity.HasIndex(e => e.DistrictCode, "idx_wards_district");

                entity.HasIndex(e => e.AdministrativeUnitId, "idx_wards_unit");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .HasColumnName("code");
                entity.Property(e => e.AdministrativeUnitId).HasColumnName("administrative_unit_id");
                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .HasColumnName("code_name");
                entity.Property(e => e.DistrictCode)
                    .HasMaxLength(20)
                    .HasColumnName("district_code");
                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");
                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("full_name_en");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .HasColumnName("name_en");

                entity.HasOne(d => d.AdministrativeUnit).WithMany(p => p.Wards)
                    .HasForeignKey(d => d.AdministrativeUnitId)
                    .HasConstraintName("wards_administrative_unit_id_fkey");

                entity.HasOne(d => d.DistrictCodeNavigation).WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictCode)
                    .HasConstraintName("wards_district_code_fkey");
            });
        }
    }
}
