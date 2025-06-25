using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace BusinessObjects.ModelsDTO.Product
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public string ProductImage { get; set; } = null!;
        public int StatusProductId { get; set; }
        public string? StatusProductName { get; set; }

        public int? ProductSoldCount { get; set; }
        public int? ProductLike { get; set; }

        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public List<ProductVariantResponse> ProductVariants { get; set; } = new();
    }
}
