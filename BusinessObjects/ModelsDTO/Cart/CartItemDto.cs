using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ModelsDTO.Cart
{
    public class CartItemDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
