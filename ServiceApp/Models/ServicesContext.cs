using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ServiceApp.Models
{
    public partial class ServicesContext : DbContext
    {
        public ServicesContext()
        {
        }

        public ServicesContext(DbContextOptions<ServicesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<ClientService> ClientServices { get; set; } = null!;
        public virtual DbSet<DocumentByService> DocumentByServices { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductPhoto> ProductPhotos { get; set; } = null!;
        public virtual DbSet<ProductSale> ProductSales { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServicePhoto> ServicePhotos { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-MAPTQIR\\MSSQLSERVER01;Database=Services;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.GenderCode)
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.PhotoPath).HasMaxLength(1000);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.HasOne(d => d.GenderCodeNavigation)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.GenderCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Client_Gender");

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Clients)
                    .UsingEntity<Dictionary<string, object>>(
                        "TagOfClient",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TagOfClient_Tag"),
                        r => r.HasOne<Client>().WithMany().HasForeignKey("ClientId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TagOfClient_Client"),
                        j =>
                        {
                            j.HasKey("ClientId", "TagId");

                            j.ToTable("TagOfClient");

                            j.IndexerProperty<int>("ClientId").HasColumnName("ClientID");

                            j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                        });
            });

            modelBuilder.Entity<ClientService>(entity =>
            {
                entity.ToTable("ClientService");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientServices)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientService_Client");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ClientServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientService_Service");
            });

            modelBuilder.Entity<DocumentByService>(entity =>
            {
                entity.ToTable("DocumentByService");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientServiceId).HasColumnName("ClientServiceID");

                entity.Property(e => e.DocumentPath).HasMaxLength(1000);

                entity.HasOne(d => d.ClientService)
                    .WithMany(p => p.DocumentByServices)
                    .HasForeignKey(d => d.ClientServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentByService_ClientService");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Gender");

                entity.Property(e => e.Code)
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(10);
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("Manufacturer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cost).HasColumnType("money");

                entity.Property(e => e.MainImagePath).HasMaxLength(1000);

                entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK_Product_Manufacturer");

                entity.HasMany(d => d.AttachedProducts)
                    .WithMany(p => p.MainProducts)
                    .UsingEntity<Dictionary<string, object>>(
                        "AttachedProduct",
                        l => l.HasOne<Product>().WithMany().HasForeignKey("AttachedProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AttachedProduct_Product1"),
                        r => r.HasOne<Product>().WithMany().HasForeignKey("MainProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AttachedProduct_Product"),
                        j =>
                        {
                            j.HasKey("MainProductId", "AttachedProductId");

                            j.ToTable("AttachedProduct");

                            j.IndexerProperty<int>("MainProductId").HasColumnName("MainProductID");

                            j.IndexerProperty<int>("AttachedProductId").HasColumnName("AttachedProductID");
                        });

                entity.HasMany(d => d.MainProducts)
                    .WithMany(p => p.AttachedProducts)
                    .UsingEntity<Dictionary<string, object>>(
                        "AttachedProduct",
                        l => l.HasOne<Product>().WithMany().HasForeignKey("MainProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AttachedProduct_Product"),
                        r => r.HasOne<Product>().WithMany().HasForeignKey("AttachedProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AttachedProduct_Product1"),
                        j =>
                        {
                            j.HasKey("MainProductId", "AttachedProductId");

                            j.ToTable("AttachedProduct");

                            j.IndexerProperty<int>("MainProductId").HasColumnName("MainProductID");

                            j.IndexerProperty<int>("AttachedProductId").HasColumnName("AttachedProductID");
                        });
            });

            modelBuilder.Entity<ProductPhoto>(entity =>
            {
                entity.ToTable("ProductPhoto");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PhotoPath).HasMaxLength(1000);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPhotos)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductPhoto_Product");
            });

            modelBuilder.Entity<ProductSale>(entity =>
            {
                entity.ToTable("ProductSale");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientServiceId).HasColumnName("ClientServiceID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.SaleDate).HasColumnType("datetime");

                entity.HasOne(d => d.ClientService)
                    .WithMany(p => p.ProductSales)
                    .HasForeignKey(d => d.ClientServiceId)
                    .HasConstraintName("FK_ProductSale_ClientService");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSales)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSale_Product");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cost).HasColumnType("money");

                entity.Property(e => e.MainImagePath).HasMaxLength(1000);

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<ServicePhoto>(entity =>
            {
                entity.ToTable("ServicePhoto");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PhotoPath).HasMaxLength(1000);

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServicePhotos)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServicePhoto_Service");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Color)
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.Property(e => e.Title).HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
