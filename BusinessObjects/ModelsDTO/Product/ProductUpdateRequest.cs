using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ModelsDTO.Product
{
    public class ProductUpdateRequest
    {
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public string ProductImage { get; set; } = null!;
        public int CreatedBy { get; set; }

        public List<ProductVariantCreateRequest> ProductVariants { get; set; } = new();
    }
}
