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

        public ProductsController()
        {
            _context = new Prn222BeverageWebsiteProjectContext();
            _shopService = new ShopService();
            _configDataService = new ConfigDataService();
            _userService = new UserService();
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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

                _context.Products.Add(newProduct);
                _context.SaveChanges();

                return Ok(new { message = "Tạo sản phẩm thành công", productId = newProduct.ProductId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo sản phẩm", error = ex.Message });
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
