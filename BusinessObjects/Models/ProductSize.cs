using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ProductSize
{
    public int ProductSizeId { get; set; }

    public string? ProductSizeName { get; set; }

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
