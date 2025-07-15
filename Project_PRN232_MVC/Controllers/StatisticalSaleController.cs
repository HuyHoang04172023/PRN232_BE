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
                .Where(o => o.ShopId == shopId && o.StatusOrder.StatusOrderName.ToLower() == "completed")
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

        [HttpGet("top-products/{shopId}")]
        public async Task<IActionResult> GetTopProducts(int shopId)
        {
            var orders = await _context.Orders
                .Where(o => o.ShopId == shopId && o.StatusOrder.StatusOrderName.ToLower() == "completed")
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

        [HttpGet("daily-revenue/{shopId}")]
        public async Task<IActionResult> GetDailyRevenue(int shopId, int year, int month)
        {
            var orders = await _context.Orders
                .Where(o => o.ShopId == shopId &&
                            o.CreatedAt.Year == year &&
                            o.CreatedAt.Month == month &&
                            o.StatusOrder.StatusOrderName.ToLower() == "completed")
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

    }
}
