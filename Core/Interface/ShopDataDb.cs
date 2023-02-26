using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shop.ProductTestWork.Core.Class;

namespace Shop.ProductTestWork.Core.Interface;

public partial class ShopDataDb : DbContext
{
    public ShopDataDb()
    {
    }

    public ShopDataDb(DbContextOptions<ShopDataDb> options)
        : base(options)
    {
    }
    public virtual DbSet<ProductImage> ProductImage {get; set; }
    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDataForOption> ProductDataForOptions { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ProductTypeDataOption> ProductTypeDataOptions { get; set; }

    public virtual DbSet<ProductUseProductType> ProductUseProductTypes { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source= Shop.Data.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity
                //.HasNoKey()
                .ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("ID");

        
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity
                //.HasNoKey()
                .ToTable("Image");

            entity.Property(e => e.ProductId).HasColumnName("ProductId");
           // entity.Property(e => e.ImageByte).HasColumnName("ImageByte");

        });


        modelBuilder.Entity<ProductDataForOption>(entity =>
        {
            entity
                //.HasNoKey()
                .ToTable("ProductDataForOption");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdProductUseProductType).HasColumnName("IdProductUseProductType");
            entity.Property(e => e.IdProductTypeDataOptions).HasColumnName("IDProductTypeDataOptions");
            
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity
                //.HasNoKey()
                .ToTable("ProductType");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<ProductTypeDataOption>(entity =>
        {
            //entity.HasNoKey();
            entity
                //.HasNoKey()
                .ToTable("ProductTypeDataOption");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdType).HasColumnName("IDType");
            //entity.Property(e => e.Caption).HasColumnName("Caption");

        });

        modelBuilder.Entity<ProductUseProductType>(entity =>
        {
            entity
                //.HasNoKey()
                .ToTable("ProductUseProductType");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdProduct).HasColumnName("IDProduct");
            entity.Property(e => e.IdProductType).HasColumnName("IDProductType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
