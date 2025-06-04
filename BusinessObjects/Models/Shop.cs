using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Shop
{
    public int ShopId { get; set; }

    public string? ShopName { get; set; }

    public string? ShopAddress { get; set; }

    public string? ShopImage { get; set; }

    public string? ShopDescription { get; set; }

    public int? StatusShopId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? ApprovedBy { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<ShopReview> ShopReviews { get; set; } = new List<ShopReview>();

    public virtual StatusShop? StatusShop { get; set; }
}
