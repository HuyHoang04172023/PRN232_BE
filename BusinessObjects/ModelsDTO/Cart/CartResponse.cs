using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ModelsDTO.Cart
{
    public class CartResponse
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemResponse> CartItems { get; set; }
    }

    public class CartItemResponse
    {
        public int CartItemId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public ProductVariantResponse ProductVariant { get; set; }
    }

    public class ProductVariantResponse
    {
        public decimal ProductVariantPrice { get; set; }
        public ProductSizeResponse ProductSize { get; set; }
        public ProductResponse Product { get; set; }
    }

    public class ProductSizeResponse
    {
        public string ProductSizeName { get; set; }
    }

    public class ProductResponse
    {
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
    }

}
