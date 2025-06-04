using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int ShopId { get; set; }

    public int StatusOrderId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string OrderNote { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Shop Shop { get; set; } = null!;

    public virtual StatusOrder StatusOrder { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
