using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ShopReview
{
    public int ShopReviewId { get; set; }

    public int UserId { get; set; }

    public int ShopId { get; set; }

    public int ShopReviewRating { get; set; }

    public string ShopReviewComment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Shop Shop { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
