using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;
using Storev3.Models;
using Storev3.SeedData;

namespace Storev3
{
    public partial class storeContext : DbContext
    {
        public storeContext()
        {

        }

        public storeContext(DbContextOptions<storeContext> options)
            : base(options)
        {
           
        }

        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Pointofissue> Pointofissues { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Productorder> Productorders { get; set; } = null!;
        public virtual DbSet<Productstorage> Productstorages { get; set; } = null!;
        public virtual DbSet<Storage> Storages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Userscore> Userscores { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=store;Username=postgres;Password=pass");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasPostgresEnum("agerestrictions", new[] { "0+", "6+", "8+", "12+", "16+", "18+" });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brand", "production");

                entity.Property(e => e.Brandid)
                    .ValueGeneratedNever()
                    .HasColumnName("brandid");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre", "production");

                entity.Property(e => e.Genreid)
                    .ValueGeneratedNever()
                    .HasColumnName("genreid");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("manufacturer", "production");

                entity.Property(e => e.Manufacturerid)
                    .ValueGeneratedNever()
                    .HasColumnName("manufacturerid");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order", "production");

                entity.Property(e => e.Orderid)
                    .ValueGeneratedNever()
                    .HasColumnName("orderid");

                entity.Property(e => e.Datedelivery).HasColumnName("datedelivery");

                entity.Property(e => e.Pointid).HasColumnName("pointid");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Userid).HasColumnName("userid");
                entity.Property(e => e.Fullcost).HasColumnName("fullcost");

                entity.HasOne(d => d.Point)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Pointid)
                    .HasConstraintName("order_pointid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("order_userid_fkey");
            });

            modelBuilder.Entity<Pointofissue>(entity =>
            {
                entity.HasKey(e => e.Pointid)
                    .HasName("pk_pointofissue");

                entity.ToTable("pointofissue", "production");

                entity.Property(e => e.Pointid)
                    .ValueGeneratedNever()
                    .HasColumnName("pointid");

                entity.Property(e => e.Address).HasColumnName("address");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product", "production");

                entity.Property(e => e.Productid)
                    .ValueGeneratedNever()
                    .HasColumnName("productid");

                entity.Property(e => e.Brandid).HasColumnName("brandid");

                entity.Property(e => e.Cost)
                    .HasColumnType("money")
                    .HasColumnName("cost");

                entity.Property(e => e.Genreid).HasColumnName("genreid");

                entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");

                entity.Property(e => e.Age).HasColumnName("age")
                .HasColumnType("agerestrictions");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Reservation).HasColumnName("reservation");

                entity.Property(e => e.Score).HasColumnName("score");
                entity.Property(e => e.Image).HasColumnName("image");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.Brandid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("product_brandid_fkey");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.Genreid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("product_genreid_fkey");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.Manufacturerid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("product_manufacturerid_fkey");
            });

            modelBuilder.Entity<Productorder>(entity =>
            {
                entity.ToTable("productorder", "production");

                entity.Property(e => e.Productorderid)
                    .ValueGeneratedNever()
                    .HasColumnName("productorderid");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Orderid).HasColumnName("orderid");

                entity.Property(e => e.Productid).HasColumnName("productid");
                

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Productorders)
                    .HasForeignKey(d => d.Orderid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("productorder_orderid_fkey");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Productorders)
                    .HasForeignKey(d => d.Productid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("productorder_productid_fkey");
            });

            modelBuilder.Entity<Productstorage>(entity =>
            {
                entity.ToTable("productstorage", "production");

                entity.Property(e => e.Productstorageid)
                    .ValueGeneratedNever()
                    .HasColumnName("productstorageid");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Storageid).HasColumnName("storageid");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Productstorages)
                    .HasForeignKey(d => d.Productid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("productstorage_productid_fkey");

                entity.HasOne(d => d.Storage)
                    .WithMany(p => p.Productstorages)
                    .HasForeignKey(d => d.Storageid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("productstorage_storageid_fkey");
            });

            modelBuilder.Entity<Userscore>(entity =>
            {
                entity.ToTable("userscore", "production");
                entity.Property(e => e.Userscoreid).HasColumnName("userscoreid");
                entity.Property(e => e.Userid).HasColumnName("userid");
                entity.Property(e => e.Productid).HasColumnName("productid");
                entity.Property(e => e.Score).HasColumnName("score");

                entity.HasOne(d => d.User)
                .WithMany(p => p.Userscores)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasConstraintName("fk_userid");

                entity.HasOne(d => d.Product)
               .WithMany(p => p.Userscores)
               .HasForeignKey(d => d.Productid)
               .OnDelete(DeleteBehavior.ClientNoAction)
               .HasConstraintName("fk_productid");
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.ToTable("storage", "production");

                entity.Property(e => e.Storageid)
                    .ValueGeneratedNever()
                    .HasColumnName("storageid");

                entity.Property(e => e.Address).HasColumnName("address");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "production");

                entity.Property(e => e.Userid)
                    .ValueGeneratedNever()
                    .HasColumnName("userid");

                entity.Property(e => e.Login).HasColumnName("login");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Patronymic).HasColumnName("patronymic");

                entity.Property(e => e.Phone)
                    .HasMaxLength(13)
                    .HasColumnName("phone")
                    .IsFixedLength();

                entity.Property(e => e.Surname).HasColumnName("surname");
                entity.Property(e => e.Role).HasColumnName("role");
                entity.HasOne(d => d.Roleid)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_role");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role", "production");
                entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
