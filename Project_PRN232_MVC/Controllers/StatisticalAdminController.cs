﻿using BusinessObjects.Models;
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

        //cac api thong ke ve shop
        [HttpGet("shop/shops-by-status")]
        public IActionResult GetShopsByStatus()
        {
            var result = _context.Shops
                .Include(s => s.StatusShop)
                .GroupBy(s => s.StatusShop.StatusShopName)
                .Select(g => new {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("shop/revenue-by-shop-variant")]
        public IActionResult GetRevenueByShop2()
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

        [HttpGet("shop/orders-by-shop")]
        public IActionResult GetOrdersByShop2()
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

        [HttpGet("shop/products-by-shop")]
        public IActionResult GetProductsByShop()
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

        [HttpGet("shop/new-shops-per-month")]
        public IActionResult GetNewShopsPerMonth()
        {
            var result = _context.Shops
                .Where(s => s.CreatedAt != null)
                .GroupBy(s => new { s.CreatedAt!.Value.Year, s.CreatedAt!.Value.Month })
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

        [HttpGet("shop/top-rated-shops")]
        public IActionResult GetTopRatedShops()
        {
            var result = _context.ShopReviews
                .Include(sr => sr.Shop)
                .ToList()
                .GroupBy(sr => sr.Shop.ShopName)
                .Select(g => new {
                    ShopName = g.Key,
                    ReviewCount = g.Count(),
                    AverageRating = Math.Round(g.Average(x => x.ShopReviewRating), 2)
                })
                .OrderByDescending(x => x.AverageRating)
                .Take(10)
                .ToList();

            return Ok(result);
        }

        // cac api thong ke ve user
        [HttpGet("user/users-by-role")]
        public IActionResult GetUsersByRole()
        {
            var result = _context.Users
                .Include(u => u.Role)
                .GroupBy(u => u.Role.RoleName)
                .Select(g => new {
                    Role = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("user/new-users-per-month")]
        public IActionResult GetNewUsersPerMonth()
        {
            var result = _context.Users
                .Where(u => u.CreatedAt != null)
                .GroupBy(u => new { u.CreatedAt!.Value.Year, u.CreatedAt!.Value.Month })
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

        [HttpGet("user/top-users-by-orders")]
        public IActionResult GetTopUsersByOrders()
        {
            var result = _context.Orders
                .Include(o => o.User)
                .ToList()
                .GroupBy(o => new {
                    o.User.UserId,
                    o.User.UserName
                })
                .Select(g => new {
                    UserId = g.Key.UserId,
                    UserName = g.Key.UserName,
                    TotalOrders = g.Count()
                })
                .OrderByDescending(x => x.TotalOrders)
                .Take(10)
                .ToList();

            return Ok(result);
        }

        [HttpGet("user/top-users-by-reviews")]
        public IActionResult GetTopUsersByReviews()
        {
            var result = _context.Reviews
                .Include(r => r.User)
                .ToList()
                .GroupBy(r => new {
                    r.User.UserId,
                    r.User.UserName
                })
                .Select(g => new {
                    UserId = g.Key.UserId,
                    UserName = g.Key.UserName,
                    ReviewCount = g.Count()
                })
                .OrderByDescending(x => x.ReviewCount)
                .Take(10)
                .ToList();

            return Ok(result);
        }

        [HttpGet("user/top-users-by-likes")]
        public IActionResult GetTopUsersByLikes()
        {
            var result = _context.Likes
                .Include(l => l.User)
                .ToList()
                .GroupBy(l => new {
                    l.User.UserId,
                    l.User.UserName
                })
                .Select(g => new {
                    UserId = g.Key.UserId,
                    UserName = g.Key.UserName,
                    LikeCount = g.Count()
                })
                .OrderByDescending(x => x.LikeCount)
                .Take(10)
                .ToList();

            return Ok(result);
        }

        [HttpGet("user/shops-by-user")]
        public IActionResult GetShopsCreatedByUser()
        {
            var result = _context.Shops
                .Where(s => s.CreatedBy != null)
                .ToList()
                .GroupBy(s => s.CreatedBy)
                .Select(g => new {
                    UserId = g.Key,
                    ShopCount = g.Count()
                })
                .Join(_context.Users,
                    g => g.UserId,
                    u => u.UserId,
                    (g, u) => new {
                        u.UserId,
                        u.UserName,
                        g.ShopCount
                    })
                .OrderByDescending(x => x.ShopCount)
                .Take(10)
                .ToList();

            return Ok(result);
        }
    }
}
