using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laptop.Models
{
    public partial class LaptopContext : IdentityDbContext<AppUserModel>
    {
        public LaptopContext()
        {
        }

        public LaptopContext(DbContextOptions<LaptopContext> options)
            : base(options)
        {
        }

       //dddd
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Color> Colors { get; set; } = null!;
        public virtual DbSet<Invoice> Invoices { get; set; } = null!;
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrdersDetail> OrdersDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductItem> ProductItems { get; set; } = null!;
        public virtual DbSet<ProductVariation> ProductVariations { get; set; } = null!;
        public virtual DbSet<Ram> Rams { get; set; } = null!;
        public virtual DbSet<Ssd> Ssds { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=NGHIANGHIA\\SQLSEVER2020EV;Initial Catalog=Laptop2;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.BrandName).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Color");

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.ColorName).HasMaxLength(50);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Import_Note_Suppiler");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasKey(e => new { e.ProductVarId, e.InvoiceId });

                entity.HasIndex(e => e.ProductVarId, "IX_InvoiceDetails")
                    .IsUnique();

                entity.Property(e => e.ProductVarId).HasColumnName("ProductVarID");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetails_Invoice1");

                entity.HasOne(d => d.ProductVar)
                    .WithOne(p => p.InvoiceDetail)
                    .HasPrincipalKey<ProductVariation>(p => p.ProductVarId)
                    .HasForeignKey<InvoiceDetail>(d => d.ProductVarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetails_ProductVariation");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(450)
                    .HasColumnName("CustomerID");

                entity.Property(e => e.Voucher).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_AspNetUsers");

                entity.HasOne(d => d.VoucherNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Voucher)
                    .HasConstraintName("FK_Orders_Voucher1");
            });

            modelBuilder.Entity<OrdersDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductVarId })
                    .HasName("PK_OrdersDetails_1");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductVarId).HasColumnName("ProductVarID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrdersDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_Details_Invoice");

                entity.HasOne(d => d.ProductVar)
                    .WithMany(p => p.OrdersDetails)
                    .HasPrincipalKey(p => p.ProductVarId)
                    .HasForeignKey(d => d.ProductVarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdersDetails_ProductVariation");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.HasOne(d => d.BrandNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.Brand)
                    .HasConstraintName("FK_Product_Brand");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<ProductItem>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ColorId })
                    .HasName("PK_ProductItems_1");

                entity.HasIndex(e => e.ProductItemsId, "IX_ProductItems")
                    .IsUnique();

                entity.HasIndex(e => e.ProductCode, "ProductCode")
                    .IsUnique();

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.ProductCode).HasMaxLength(50);

                entity.Property(e => e.ProductItemsId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ProductItemsID");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.ProductItems)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductItems_Color");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductItems_Product");
            });

            modelBuilder.Entity<ProductVariation>(entity =>
            {
                entity.HasKey(e => new { e.ProductItemsId, e.RamId, e.Ssdid })
                    .HasName("PK_ProductVariation_1");

                entity.ToTable("ProductVariation");

                entity.HasIndex(e => e.ProductVarId, "IX_ProductVariation")
                    .IsUnique();

                entity.Property(e => e.ProductItemsId).HasColumnName("ProductItemsID");

                entity.Property(e => e.RamId).HasColumnName("RamID");

                entity.Property(e => e.Ssdid).HasColumnName("SSDID");

                entity.Property(e => e.ProductVarId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ProductVarID");

                entity.HasOne(d => d.ProductItems)
                    .WithMany(p => p.ProductVariations)
                    .HasPrincipalKey(p => p.ProductItemsId)
                    .HasForeignKey(d => d.ProductItemsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductVariation_ProductItems");

                entity.HasOne(d => d.Ram)
                    .WithMany(p => p.ProductVariations)
                    .HasForeignKey(d => d.RamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductVariation_Size");

                entity.HasOne(d => d.Ssd)
                    .WithMany(p => p.ProductVariations)
                    .HasForeignKey(d => d.Ssdid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductVariation_SSD");
            });

            modelBuilder.Entity<Ram>(entity =>
            {
                entity.ToTable("Ram");

                entity.Property(e => e.RamId).HasColumnName("RamID");

                entity.Property(e => e.RamName).HasMaxLength(50);
            });

            modelBuilder.Entity<Ssd>(entity =>
            {
                entity.ToTable("SSD");

                entity.Property(e => e.SsdId).HasColumnName("SsdID");

                entity.Property(e => e.Ssdname)
                    .HasMaxLength(50)
                    .HasColumnName("SSDName");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(30);

                entity.Property(e => e.SupplierName).HasMaxLength(50);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasKey(e => e.VoucherCode);

                entity.ToTable("Voucher");

                entity.Property(e => e.VoucherCode).HasMaxLength(50);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
