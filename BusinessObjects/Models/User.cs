using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> ProductApprovedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Shop> ShopApprovedByNavigations { get; set; } = new List<Shop>();

    public virtual ICollection<Shop> ShopCreatedByNavigations { get; set; } = new List<Shop>();

    public virtual ICollection<ShopReview> ShopReviews { get; set; } = new List<ShopReview>();
}
