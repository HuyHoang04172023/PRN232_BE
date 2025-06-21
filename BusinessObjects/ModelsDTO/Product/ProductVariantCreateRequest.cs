using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ModelsDTO.Product
{
    public class ProductVariantCreateRequest
    {
        public decimal ProductVariantPrice { get; set; }
        public int ProductSizeId { get; set; }
    }
}
