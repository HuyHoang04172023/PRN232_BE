using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.Implement;
using Services.Interface;
using BusinessObjects.ModelsDTO.Product;

namespace Project_PRN232_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        private readonly ShopService _shopService;
        private readonly ConfigDataService _configDataService;
        private readonly UserService _userService;
        private readonly ProductService _productService;

        public ProductsController()
        {
            _context = new Prn222BeverageWebsiteProjectContext();
            _shopService = new ShopService();
            _configDataService = new ConfigDataService();
            _userService = new UserService();
            _productService = new ProductService();
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{productId}")]
        public IActionResult GetProductById(int productId)
        {
            try
            {
                Product? product = _productService.GetProductByProductId(productId);

                if (product == null)
                {
                    return NotFound(new { message = "Không tìm thấy sản phẩm." });
                }

                var response = new ProductResponse
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductImage = product.ProductImage,
                    StatusProductId = product.StatusProductId,
                    StatusProductName = product.StatusProduct?.StatusProductName,
                    ProductSoldCount = product.ProductSoldCount,
                    ProductLike = product.ProductLike,
                    ProductVariants = product.ProductVariants.Select(v => new ProductVariantResponse
                    {
                        ProductVariantId = v.ProductVariantId,
                        ProductVariantPrice = v.ProductVariantPrice,
                        ProductSizeId = v.ProductSizeId
                    }).ToList()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy sản phẩm", error = ex.Message });
            }
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductCreateRequest request)
        {
            if (request.ProductVariants == null || !request.ProductVariants.Any())
            {
                return BadRequest(new { message = "Cần ít nhất một biến thể sản phẩm." });
            }

            var duplicateSizeIds = request.ProductVariants
                .GroupBy(v => v.ProductSizeId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateSizeIds.Any())
            {
                return BadRequest(new
                {
                    message = "Mỗi size chỉ được xuất hiện một lần trong biến thể sản phẩm.",
                    duplicatedSizes = duplicateSizeIds
                });
            }

            try
            {
                var shopId = _shopService.GetShopIdByUserId(request.CreatedBy);

                if (shopId == null)
                {
                    return BadRequest(new { message = "Người dùng hiện chưa có cửa hàng nào." });
                }

                var newProduct = new Product
                {
                    ProductName = request.ProductName,
                    ProductDescription = request.ProductDescription,
                    ProductImage = request.ProductImage,
                    StatusProductId = (int)_configDataService.GetStatusProductIdByStatusProductName("pending"),
                    ShopId = shopId.Value,
                    CreatedBy = request.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                };

                foreach (var variant in request.ProductVariants)
                {
                    newProduct.ProductVariants.Add(new ProductVariant
                    {
                        ProductVariantPrice = variant.ProductVariantPrice,
                        ProductSizeId = variant.ProductSizeId
                    });
                }

                _productService.CreateProduct(newProduct);

                return Ok(new { message = "Tạo sản phẩm thành công", productId = newProduct.ProductId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi tạo sản phẩm",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductUpdateRequest request)
        {
            if (request.ProductVariants == null || !request.ProductVariants.Any())
            {
                return BadRequest(new { message = "Cần ít nhất một biến thể sản phẩm." });
            }

            // ✅ Validate không trùng size
            var duplicateSizeIds = request.ProductVariants
                .GroupBy(v => v.ProductSizeId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateSizeIds.Any())
            {
                return BadRequest(new
                {
                    message = "Mỗi size chỉ được xuất hiện một lần trong biến thể sản phẩm.",
                    duplicatedSizes = duplicateSizeIds
                });
            }

            try
            {
                var product = _productService.GetProductByProductId(productId);

                if (product == null)
                {
                    return NotFound(new { message = "Không tìm thấy sản phẩm cần cập nhật." });
                }

                product.ProductName = request.ProductName;
                product.ProductDescription = request.ProductDescription;
                product.ProductImage = request.ProductImage;

                var existingVariants = product.ProductVariants.ToList();
                var incomingSizeIds = request.ProductVariants.Select(v => v.ProductSizeId).ToList();

                foreach (var variant in existingVariants)
                {
                    if (!incomingSizeIds.Contains(variant.ProductSizeId))
                    {
                        bool isInUse = _context.OrderItems.Any(o => o.ProductVariantId == variant.ProductVariantId);
                        if (!isInUse)
                        {
                            _context.ProductVariants.Remove(variant);
                        }
                    }
                }

                foreach (var variantRequest in request.ProductVariants)
                {
                    var existing = existingVariants
                        .FirstOrDefault(v => v.ProductSizeId == variantRequest.ProductSizeId);

                    if (existing != null)
                    {
                        existing.ProductVariantPrice = variantRequest.ProductVariantPrice;
                    }
                    else
                    {
                        product.ProductVariants.Add(new ProductVariant
                        {
                            ProductSizeId = variantRequest.ProductSizeId,
                            ProductVariantPrice = variantRequest.ProductVariantPrice,
                            ProductId = product.ProductId
                        });
                    }
                }

                _context.SaveChanges();

                return Ok(new { message = "Cập nhật sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi cập nhật sản phẩm",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                var product = _productService.GetProductByProductId(productId);

                if (product == null)
                {
                    return NotFound(new { message = "Không tìm thấy sản phẩm cần xóa." });
                }

                bool result = _productService.DeleteProduct(product);

                if (!result)
                {
                    return BadRequest(new { message = "Không thể xóa sản phẩm. Có thể sản phẩm đang được sử dụng ở nơi khác." });
                }

                return Ok(new { message = "Xóa sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa sản phẩm", error = ex.Message });
            }
        }

        [HttpGet("status/{statusName}")]
        public IActionResult GetProductsByStatusProductName(string statusName)
        {
            try
            {
                var status = _context.StatusProducts
                    .FirstOrDefault(s => s.StatusProductName.ToLower() == statusName.ToLower());

                if (status == null)
                {
                    return NotFound(new { message = "Không tìm thấy trạng thái sản phẩm." });
                }

                var products = _context.Products
                    .Where(p => p.StatusProductId == status.StatusProductId)
                    .Include(p => p.ProductVariants)
                    .Include(p => p.Shop)
                    .Include(p => p.StatusProduct)
                    .ToList();

                var response = products.Select(product => new ProductResponse
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductImage = product.ProductImage,
                    StatusProductId = product.StatusProductId,
                    StatusProductName = product.StatusProduct?.StatusProductName,
                    ProductSoldCount = product.ProductSoldCount,
                    ProductLike = product.ProductLike,
                    ShopId = product.ShopId,
                    ShopName = product.Shop.ShopName,
                    ProductVariants = product.ProductVariants.Select(v => new ProductVariantResponse
                    {
                        ProductVariantId = v.ProductVariantId,
                        ProductVariantPrice = v.ProductVariantPrice,
                        ProductSizeId = v.ProductSizeId
                    }).ToList()
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách sản phẩm", error = ex.Message });
            }
        }

        [HttpPost("update-status/{productId}")]
        public async Task<IActionResult> UpdateProductStatus(int productId, string statusName)
        {
            bool result = false;

            int? statusId = _configDataService.GetStatusProductIdByStatusProductName(statusName);
            result = _productService.UpdateStatusProductIdByProductId(productId, (int) statusId);

            if (result)
            {
                return Ok(new { message = "Cập nhật trạng thái thành công." });
            }
            else
            {
                return NotFound(new { message = "Không tìm thấy cửa hàng hoặc lỗi khi cập nhật." });
            }
        }
    }
}
