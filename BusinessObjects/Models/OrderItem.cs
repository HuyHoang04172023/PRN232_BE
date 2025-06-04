using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductVariantId { get; set; }

    public int OrderItemQuantity { get; set; }

    public decimal OrderItemPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ProductVariant ProductVariant { get; set; } = null!;
}
