using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class StatusProduct
{
    public int StatusProductId { get; set; }

    public string StatusProductName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
