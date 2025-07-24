using BusinessObjects.Models;
using BusinessObjects.ModelsDTO.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Implement;

namespace Project_PRN232_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        private readonly ConfigDataService _configDataService;

        public OrderController()
        {
            _context = new Prn222BeverageWebsiteProjectContext();
            _configDataService = new ConfigDataService();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound("User not found");

            var order = new Order
            {
                UserId = request.UserId,
                ShopId = request.ShopId,
                ShippingAddress = request.ShippingAddress,
                PhoneNumber = request.PhoneNumber,
                OrderNote = request.OrderNote,
                CreatedAt = DateTime.UtcNow,
                StatusOrderId = 1
            };

            var variantIdsInOrder = new List<int>();

            foreach (var item in request.Items)
            {
                var variant = await _context.ProductVariants.FindAsync(item.ProductVariantId);
                if (variant == null)
                    return BadRequest($"ProductVariantId {item.ProductVariantId} not found");

                var orderItem = new OrderItem
                {
                    ProductVariantId = variant.ProductVariantId,
                    OrderItemQuantity = item.Quantity,
                    OrderItemPrice = variant.ProductVariantPrice
                };

                order.OrderItems.Add(orderItem);
                variantIdsInOrder.Add(variant.ProductVariantId);
            }

            _context.Orders.Add(order);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            if (cart != null)
            {
                var itemsToRemove = cart.CartItems
                    .Where(ci => variantIdsInOrder.Contains(ci.ProductVariantId))
                    .ToList();

                _context.CartItems.RemoveRange(itemsToRemove);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Order created successfully", orderId = order.OrderId });
        }

        [HttpGet("shop/{shopId}")]
        public async Task<IActionResult> GetOrdersByShop(int shopId)
        {
            var orders = await _context.Orders
                .Where(o => o.ShopId == shopId)
                .Include(o => o.User)
                .Include(o => o.StatusOrder)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                .ToListAsync();

            if (!orders.Any())
                return NotFound($"No orders found for shopId = {shopId}");

            var result = orders.Select(o => new
            {
                o.OrderId,
                o.CreatedAt,
                o.ShippingAddress,
                o.PhoneNumber,
                o.OrderNote,
                Status = o.StatusOrder.StatusOrderName,
                Customer = new
                {
                    o.User.UserId,
                    o.User.UserName,
                    o.User.Email
                },
                Items = o.OrderItems.Select(oi => new
                {
                    oi.ProductVariantId,
                    ProductName = oi.ProductVariant.Product.ProductName,
                    oi.OrderItemQuantity,
                    oi.OrderItemPrice
                })
            });

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.StatusOrder)
                .Include(o => o.Shop)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            if (!orders.Any())
                return NotFound($"No orders found for userId = {userId}");

            var result = orders.Select(o => new
            {
                o.OrderId,
                o.CreatedAt,
                o.ShippingAddress,
                o.PhoneNumber,
                o.OrderNote,
                Status = o.StatusOrder.StatusOrderName,
                Shop = new
                {
                    o.Shop.ShopId,
                    o.Shop.ShopName
                },
                Items = o.OrderItems.Select(oi => new
                {
                    oi.ProductVariantId,
                    ProductName = oi.ProductVariant.Product.ProductName,
                    oi.OrderItemQuantity,
                    oi.OrderItemPrice
                })
            });

            return Ok(result);
        }

        [HttpPut("update-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromQuery] string statusName)
        {
            var order = await _context.Orders
                .Include(o => o.StatusOrder)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound($"Order with ID {orderId} not found");

            int? statusId = _configDataService.GetStatusOrderIdByStatusOrderName(statusName);
            if (statusId == null)
                return BadRequest($"Invalid status name: {statusName}");

            order.StatusOrderId = statusId.Value;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Order status updated successfully",
                orderId = order.OrderId,
                newStatus = statusName
            });
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Shop)
                .Include(o => o.StatusOrder)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                            .ThenInclude(p => p.Shop)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.ProductSize)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound($"Order with ID {orderId} not found");

            var result = new
            {
                order.OrderId,
                order.CreatedAt,
                order.ShippingAddress,
                order.PhoneNumber,
                order.OrderNote,
                Status = order.StatusOrder.StatusOrderName,
                Customer = new
                {
                    order.User.UserId,
                    order.User.UserName,
                    order.User.Email
                },
                Shop = new
                {
                    order.Shop.ShopId,
                    order.Shop.ShopName
                },
                Items = order.OrderItems.Select(oi => new
                {
                    oi.ProductVariantId,
                    Quantity = oi.OrderItemQuantity,
                    UnitPrice = oi.OrderItemPrice,
                    Total = oi.OrderItemPrice * oi.OrderItemQuantity,
                    Product = new
                    {
                        ProductName = oi.ProductVariant.Product.ProductName,
                        ProductImage = oi.ProductVariant.Product.ProductImage,
                        ShopId = oi.ProductVariant.Product.Shop.ShopId,
                        ShopName = oi.ProductVariant.Product.Shop.ShopName,
                        ProductSize = oi.ProductVariant.ProductSize.ProductSizeName,
                        ProductPrice = oi.ProductVariant.ProductVariantPrice
                    }
                }),
                TotalAmount = order.OrderItems.Sum(oi => oi.OrderItemPrice * oi.OrderItemQuantity)
            };

            return Ok(result);
        }
    }
}
