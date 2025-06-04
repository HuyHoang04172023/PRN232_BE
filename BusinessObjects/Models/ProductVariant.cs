using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ProductVariant
{
    public int ProductVariantId { get; set; }

    public decimal ProductVariantPrice { get; set; }

    public int ProductSizeId { get; set; }

    public int ProductId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Product Product { get; set; } = null!;

    public virtual ProductSize ProductSize { get; set; } = null!;
}
