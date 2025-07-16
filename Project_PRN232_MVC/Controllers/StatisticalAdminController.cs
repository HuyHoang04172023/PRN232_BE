using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Project_PRN232_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticalAdminController : ControllerBase
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;

        public StatisticalAdminController(Prn222BeverageWebsiteProjectContext context)
        {
            _context = context;
        }
        //Thống kê về đơn hàng, Thống kê về sản phẩm, Thống kê về shop, Thống kê người dùng
        // cac api thong ke ve order
        [HttpGet("order/orders-per-month")]
        public IActionResult GetOrdersPerMonth()
        {
            var result = _context.Orders
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalOrders = g.Count()
                })
                .ToList()
                .Select(x => new {
                    Month = $"{x.Year}-{x.Month:00}",
                    x.TotalOrders
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Ok(result);
        }

        [HttpGet("order/revenue-per-month")]
        public IActionResult GetRevenuePerMonth()
        {
            var result = _context.OrderItems
                .Include(oi => oi.Order)
                .GroupBy(oi => new { oi.Order.CreatedAt.Year, oi.Order.CreatedAt.Month })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(x => x.OrderItemPrice * x.OrderItemQuantity)
                })
                .ToList()
                .Select(x => new {
                    Month = $"{x.Year}-{x.Month:00}",
                    x.Revenue
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Ok(result);
        }

        [HttpGet("order/orders-by-status")]
        public IActionResult GetOrdersByStatus()
        {
            var result = _context.Orders
                .Include(o => o.StatusOrder)
                .GroupBy(o => o.StatusOrder.StatusOrderName)
                .Select(g => new {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("order/revenue-by-shop")]
        public IActionResult GetRevenueByShop()
        {
            var result = _context.OrderItems
                .Include(oi => oi.Order)
                .ThenInclude(o => o.Shop)
                .ToList()
                .GroupBy(oi => oi.Order.Shop.ShopName)
                .Select(g => new {
                    ShopName = g.Key,
                    Revenue = g.Sum(x => x.OrderItemPrice * x.OrderItemQuantity)
                })
                .OrderByDescending(x => x.Revenue)
                .ToList();

            return Ok(result);
        }

        [HttpGet("order/orders-by-shop")]
        public IActionResult GetOrdersByShop()
        {
            var result = _context.Orders
                .Include(o => o.Shop)
                .ToList()
                .GroupBy(o => o.Shop.ShopName)
                .Select(g => new {
                    ShopName = g.Key,
                    TotalOrders = g.Count()
                })
                .OrderByDescending(x => x.TotalOrders)
                .ToList();

            return Ok(result);
        }

        [HttpGet("order/total-revenue")]
        public IActionResult GetTotalRevenue()
        {
            var total = _context.OrderItems
                .ToList()
                .Sum(oi => oi.OrderItemPrice * oi.OrderItemQuantity);

            return Ok(new { totalRevenue = total });
        }

        // cac api thong ke ve product
        [HttpGet("product/products-by-status")]
        public IActionResult GetProductsByStatus()
        {
            var result = _context.Products
                .Include(p => p.StatusProduct)
                .GroupBy(p => p.StatusProduct.StatusProductName)
                .Select(g => new {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("product/top-selling-products")]
        public IActionResult GetTopSellingProducts()
        {
            var result = _context.OrderItems
                .Include(oi => oi.ProductVariant)
                .ThenInclude(pv => pv.Product)
                .ToList()
                .GroupBy(oi => new {
                    oi.ProductVariant.Product.ProductId,
                    oi.ProductVariant.Product.ProductName
                })
                .Select(g => new {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalSold = g.Sum(x => x.OrderItemQuantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(10)
                .ToList();

            return Ok(result);
        }

        [HttpGet("product/most-liked-products")]
        public IActionResult GetMostLikedProducts()
        {
            var result = _context.Likes
                .Include(l => l.Product)
                .ToList()
                .GroupBy(l => new {
                    l.Product.ProductId,
                    l.Product.ProductName
                })
                .Select(g => new {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    LikeCount = g.Count()
                })
                .OrderByDescending(x => x.LikeCount)
                .Take(10)
                .ToList();

            return Ok(result);
        }

        [HttpGet("product/top-reviewed-products")]
        public IActionResult GetTopReviewedProducts()
        {
            var result = _context.Reviews
                .Include(r => r.Product)
                .ToList()
                .GroupBy(r => new {
                    r.Product.ProductId,
                    r.Product.ProductName
                })
                .Select(g => new {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    ReviewCount = g.Count(),
                    AverageRating = Math.Round(g.Average(x => x.ReviewRating), 2)
                })
                .OrderByDescending(x => x.ReviewCount)
                .Take(10)
                .ToList();

            return Ok(result);
        }

        [HttpGet("product/products-by-shop")]
        public IActionResult GetProductCountByShop()
        {
            var result = _context.Products
                .Include(p => p.Shop)
                .ToList()
                .GroupBy(p => p.Shop.ShopName)
                .Select(g => new {
                    ShopName = g.Key,
                    ProductCount = g.Count()
                })
                .OrderByDescending(x => x.ProductCount)
                .ToList();

            return Ok(result);
        }

        [HttpGet("product/new-products-per-month")]
        public IActionResult GetNewProductsPerMonth()
        {
            var result = _context.Products
                .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .ToList()
                .Select(x => new {
                    Month = $"{x.Year}-{x.Month:00}",
                    x.Count
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Ok(result);
        }

        
    }
}
