using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Project_PRN232_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticalSaleController : ControllerBase
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;

        public StatisticalSaleController(Prn222BeverageWebsiteProjectContext context)
        {
            _context = context;
        }

        [HttpGet("revenue-by-month/{shopId}")]
        public async Task<IActionResult> GetRevenueByMonth(int shopId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.ShopId == shopId && o.StatusOrder.StatusOrderName.ToLower() == "success")
                .ToListAsync();

            var result = orders
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new {
                    Month = $"{g.Key.Month:00}/{g.Key.Year}",
                    Revenue = g.SelectMany(o => o.OrderItems)
                               .Sum(oi => oi.OrderItemPrice * oi.OrderItemQuantity)
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Ok(result);
        }

        [HttpGet("daily-revenue/{shopId}")]
        public async Task<IActionResult> GetDailyRevenue(int shopId, int year, int month)
        {
            var orders = await _context.Orders
                .Where(o => o.ShopId == shopId &&
                            o.CreatedAt.Year == year &&
                            o.CreatedAt.Month == month &&
                            o.StatusOrder.StatusOrderName.ToLower() == "success")
                .Include(o => o.OrderItems)
                .ToListAsync();

            var result = orders
                .GroupBy(o => o.CreatedAt.Day)
                .Select(g => new {
                    Day = g.Key.ToString("00"),
                    Revenue = g.SelectMany(o => o.OrderItems)
                               .Sum(oi => oi.OrderItemPrice * oi.OrderItemQuantity)
                })
                .OrderBy(x => x.Day)
                .ToList();

            return Ok(result);
        }

        [HttpGet("top-products/{shopId}")]
        public async Task<IActionResult> GetTopProducts(int shopId)
        {
            var orders = await _context.Orders
                .Where(o => o.ShopId == shopId && o.StatusOrder.StatusOrderName.ToLower() == "success")
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                .ToListAsync();

            var result = orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.ProductVariant.Product.ProductName)
                .Select(g => new {
                    ProductName = g.Key,
                    Quantity = g.Sum(x => x.OrderItemQuantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToList();

            return Ok(result);
        }

        [HttpGet("product-likes-ratio/{shopId}")]
        public async Task<IActionResult> GetProductLikesRatio(int shopId)
        {
            var data = await _context.Products
                .Where(p => p.ShopId == shopId)
                .Select(p => new {
                    ProductName = p.ProductName,
                    Likes = p.ProductLike ?? 0
                })
                .OrderByDescending(p => p.Likes)
                .Take(5)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("orders-by-status/{shopId}")]
        public async Task<IActionResult> GetOrdersByStatus(int shopId)
        {
            var data = await _context.Orders
                .Where(o => o.ShopId == shopId)
                .Include(o => o.StatusOrder)
                .GroupBy(o => o.StatusOrder.StatusOrderName)
                .Select(g => new {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("orders/summary/{shopId}")]
        public IActionResult GetOrderSummary(int shopId)
        {
            var orders = _context.Orders
                .Where(o => o.ShopId == shopId)
                .Include(o => o.OrderItems)
                .Include(o => o.StatusOrder)
                .ToList();

            var totalOrders = orders.Count;
            var successOrders = orders.Where(o => o.StatusOrder.StatusOrderName == "success").ToList();
            var cancelledOrders = orders.Where(o => o.StatusOrder.StatusOrderName == "cancel").ToList();

            var totalRevenue = successOrders
                .SelectMany(o => o.OrderItems)
                .Sum(oi => oi.OrderItemPrice * oi.OrderItemQuantity);

            return Ok(new
            {
                TotalOrders = totalOrders,
                SuccessOrders = successOrders.Count,
                CancelledOrders = cancelledOrders.Count,
                TotalRevenue = totalRevenue
            });
        }

        [HttpGet("orders/by-month/{shopId}")]
        public IActionResult GetOrdersByMonth(int shopId)
        {
            var result = _context.Orders
                .Where(o => o.ShopId == shopId)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalOrders = g.Count()
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToList();

            return Ok(result);
        }

        [HttpGet("orders/status-monthly/{shopId}")]
        public IActionResult GetOrderStatusByMonth(int shopId)
        {
            var result = _context.Orders
                .Where(o => o.ShopId == shopId)
                .GroupBy(o => new {
                    o.CreatedAt.Year,
                    o.CreatedAt.Month,
                    Status = o.StatusOrder.StatusOrderName
                })
                .Select(g => new {
                    g.Key.Year,
                    g.Key.Month,
                    Status = g.Key.Status,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToList();

            return Ok(result);
        }

    }
}
