using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public string ProductImage { get; set; } = null!;

    public int StatusProductId { get; set; }

    public int? ProductSoldCount { get; set; }

    public int? ProductLike { get; set; }

    public int ShopId { get; set; }

    public int ApprovedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User ApprovedByNavigation { get; set; } = null!;

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Shop Shop { get; set; } = null!;

    public virtual StatusProduct StatusProduct { get; set; } = null!;
}
