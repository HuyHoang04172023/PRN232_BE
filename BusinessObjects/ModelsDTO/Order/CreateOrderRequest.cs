using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ModelsDTO.Order
{
    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public int ShopId { get; set; } // Nếu đơn hàng gắn với 1 shop cụ thể
        public string ShippingAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string OrderNote { get; set; } = "";

        public List<OrderItemRequest> Items { get; set; } = new();
    }

    public class OrderItemRequest
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }

}
