using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class Prn222BeverageWebsiteProjectContext : DbContext
{
    public Prn222BeverageWebsiteProjectContext()
    {
    }

    public Prn222BeverageWebsiteProjectContext(DbContextOptions<Prn222BeverageWebsiteProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<ShopReview> ShopReviews { get; set; }

    public virtual DbSet<StatusOrder> StatusOrders { get; set; }

    public virtual DbSet<StatusProduct> StatusProducts { get; set; }

    public virtual DbSet<StatusShop> StatusShops { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(local);database=PRN222_Beverage_Website_Project;uid=sa;pwd=123;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Users");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItem");

            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.ProductVariantId).HasColumnName("ProductVariantID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Cart");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_ProductVariants");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.Property(e => e.LikeId).HasColumnName("LikeID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.Likes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Likes_Products");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Likes_Users");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderNote).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShippingAddress).HasMaxLength(150);
            entity.Property(e => e.ShopId).HasColumnName("ShopID");
            entity.Property(e => e.StatusOrderId).HasColumnName("StatusOrderID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Shop).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Shops");

            entity.HasOne(d => d.StatusOrder).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Status");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderItemPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductVariantId).HasColumnName("ProductVariantID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_ProductVariants");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductLike).HasDefaultValue(0);
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.ProductSoldCount).HasDefaultValue(0);
            entity.Property(e => e.ShopId).HasColumnName("ShopID");
            entity.Property(e => e.StatusProductId).HasColumnName("StatusProductID");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.ProductApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Users");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Products_Users1");

            entity.HasOne(d => d.Shop).WithMany(p => p.Products)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Shops");

            entity.HasOne(d => d.StatusProduct).WithMany(p => p.Products)
                .HasForeignKey(d => d.StatusProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_StatusProduct");
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.Property(e => e.ProductSizeId).HasColumnName("ProductSizeID");
            entity.Property(e => e.ProductSizeName).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.Property(e => e.ProductVariantId).HasColumnName("ProductVariantID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductSizeId).HasColumnName("ProductSizeID");
            entity.Property(e => e.ProductVariantPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductVariants_Products");

            entity.HasOne(d => d.ProductSize).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductSizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductVariants_ProductSizes");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ReviewComment).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Products");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.Property(e => e.ShopId).HasColumnName("ShopID");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ShopImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StatusShopId).HasColumnName("StatusShopID");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.ShopApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK_Shops_Users3");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ShopCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Shops_Users2");

            entity.HasOne(d => d.StatusShop).WithMany(p => p.Shops)
                .HasForeignKey(d => d.StatusShopId)
                .HasConstraintName("FK_Shops_StatusShop");
        });

        modelBuilder.Entity<ShopReview>(entity =>
        {
            entity.Property(e => e.ShopReviewId).HasColumnName("ShopReviewID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ShopId).HasColumnName("ShopID");
            entity.Property(e => e.ShopReviewComment).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Shop).WithMany(p => p.ShopReviews)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopReviews_Shops");

            entity.HasOne(d => d.User).WithMany(p => p.ShopReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopReviews_Users");
        });

        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.HasKey(e => e.StatusOrderId).HasName("PK_Status");

            entity.ToTable("StatusOrder");

            entity.Property(e => e.StatusOrderId).HasColumnName("StatusOrderID");
            entity.Property(e => e.StatusOrderName).HasMaxLength(50);
        });

        modelBuilder.Entity<StatusProduct>(entity =>
        {
            entity.ToTable("StatusProduct");

            entity.Property(e => e.StatusProductId).HasColumnName("StatusProductID");
            entity.Property(e => e.StatusProductName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusShop>(entity =>
        {
            entity.ToTable("StatusShop");

            entity.Property(e => e.StatusShopId).HasColumnName("StatusShopID");
            entity.Property(e => e.StatusShopName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
