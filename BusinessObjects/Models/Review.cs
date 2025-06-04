using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int ReviewRating { get; set; }

    public string ReviewComment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
