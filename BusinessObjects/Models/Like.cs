using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Like
{
    public int LikeId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
