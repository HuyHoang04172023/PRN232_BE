using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Repositories.Interface;
using Services.Interface;
using Services.Implement;
using BusinessObjects.ModelsDTO.Shop;
using Microsoft.AspNetCore.Authorization;

namespace Project_PRN232_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        private readonly ShopService _shopService;
        private readonly ConfigDataService _configDataService;
        private readonly UserService _userService;

        public ShopsController(Prn222BeverageWebsiteProjectContext context)
        {
            _context = context;
            _shopService = new ShopService();
            _configDataService = new ConfigDataService();
            _userService = new UserService();
        }

        // GET: api/Shops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopResponse>>> GetShops()
        {
            List<Shop> shops = _shopService.GetShops();

            var response = shops.Select(s => new ShopResponse
            {
                ShopId = s.ShopId,
                ShopName = s.ShopName,
                ShopAddress = s.ShopAddress,
                ShopImage = s.ShopImage,
                ShopDescription = s.ShopDescription,
                StatusShopId = s.StatusShopId,
                CreatedBy = s.CreatedBy,
                CreatedAt = s.CreatedAt,
                ApprovedBy = s.ApprovedBy,
                StatusShop = s.StatusShop != null ? s.StatusShop.StatusShopName : null
            }).ToList();

            return Ok(response);
        }

        // GET: api/shops/status/{statusId}
        [HttpGet("status/{statusName}")]
        public async Task<ActionResult<IEnumerable<ShopResponse>>> GetShopsByStatus(string statusName)
        {
            List<Shop> shops = _shopService.GetShopsByStatusName(statusName);

            if (shops == null || !shops.Any())
            {
                return NotFound(new { message = "Không tìm thấy cửa hàng với trạng thái này." });
            }

            var response = shops.Select(s => new ShopResponse
            {
                ShopId = s.ShopId,
                ShopName = s.ShopName,
                ShopAddress = s.ShopAddress,
                ShopImage = s.ShopImage,
                ShopDescription = s.ShopDescription,
                StatusShopId = s.StatusShopId,
                CreatedBy = s.CreatedBy,
                CreatedAt = s.CreatedAt,
                ApprovedBy = s.ApprovedBy,
                StatusShop = s.StatusShop != null ? s.StatusShop.StatusShopName : null
            }).ToList();

            return Ok(response);
        }

        [HttpGet("shop-info/{shopId}")]
        public IActionResult GetShopDetail(int shopId)
        {
            var shop = _context.Shops
                .Include(s => s.StatusShop)
                .Include(s => s.CreatedByNavigation)
                .FirstOrDefault(s => s.ShopId == shopId);

            if (shop == null)
            {
                return NotFound(new { message = "Không tìm thấy cửa hàng." });
            }

            var response = new ShopDetailResponse
            {
                ShopId = shop.ShopId,
                ShopName = shop.ShopName,
                ShopAddress = shop.ShopAddress,
                ShopImage = shop.ShopImage,
                ShopDescription = shop.ShopDescription,
                CreatedAt = shop.CreatedAt,
                StatusShopName = shop.StatusShop?.StatusShopName,
            };

            return Ok(response);
        }

        // POST: api/Shops
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "UserOnly")]
        [HttpPost]
        public async Task<ActionResult<Shop>> PostShop(CreateShopRequest createShopRequest)
        {
            Shop shop = new Shop();

            shop.ShopName = createShopRequest.ShopName;
            shop.ShopAddress = createShopRequest.ShopAddress;
            shop.ShopImage = createShopRequest.ShopImage;
            shop.ShopDescription = createShopRequest.ShopDescription;
            shop.StatusShopId = _configDataService.GetStatusShopIdByStatusShopName("pending");
            shop.CreatedBy = createShopRequest.CreatedBy;
            shop.CreatedAt = DateTime.Now;

            Shop createdShop = _shopService.CreateShop(shop);

            return CreatedAtAction(nameof(GetShopByUserId), new { userId = createdShop.CreatedBy }, createdShop);
        }

        // GET: api/Shops/5
        [Authorize(Policy = "UserOnly")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<ShopResponse>> GetShopByUserId(int userId)
        {
            Shop? shop = _shopService.GetShopByUserId(userId);

            if (shop == null)
            {
                return NotFound(new { message = "Không tìm thấy cửa hàng với user ID này." });
            }

            var response = new ShopResponse
            {
                ShopId = shop.ShopId,
                ShopName = shop.ShopName,
                ShopAddress = shop.ShopAddress,
                ShopImage = shop.ShopImage,
                ShopDescription = shop.ShopDescription,
                StatusShopId = shop.StatusShopId,
                CreatedBy = shop.CreatedBy,
                CreatedAt = shop.CreatedAt,
                ApprovedBy = shop.ApprovedBy,
                StatusShop = shop.StatusShop != null ? shop.StatusShop.StatusShopName : null
            };

            return Ok(response);
        }

        // PUT: api/Shops/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "UserOnly")]
        [HttpPut("{shopId}")]
        public async Task<IActionResult> PutShop(int shopId, UpdateShopRequest shopRequest)
        {
            Shop? existingShop = _shopService.GetShopByShopId(shopId);

            if (existingShop == null)
            {
                return NotFound(new { message = "Không tìm thấy cửa hàng." });
            }

            existingShop.ShopName = shopRequest.ShopName;
            existingShop.ShopAddress = shopRequest.ShopAddress;
            existingShop.ShopImage = shopRequest.ShopImage;
            existingShop.ShopDescription = shopRequest.ShopDescription;
            existingShop.StatusShopId = _configDataService.GetStatusShopIdByStatusShopName("pending");

            _shopService.UpdateShop(shopId, existingShop);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingShop);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Lỗi cập nhật dữ liệu. Vui lòng thử lại." });
            }
        }

        // DELETE: api/Shops/5
        [Authorize(Policy = "UserOnly")]
        [HttpDelete("{shopId}")]
        public IActionResult DeleteShop(int shopId)
        {
            var existingShop = _shopService.GetShopByShopId(shopId);

            if (existingShop == null)
            {
                return NotFound(new { message = "Không tìm thấy cửa hàng." });
            }

            bool isDeleted = _shopService.DeleteShop(shopId);

            if (!isDeleted)
            {
                return StatusCode(500, new { message = "Xóa cửa hàng thất bại. Vui lòng thử lại sau." });
            }

            return NoContent();
        }

        // POST: api/Shops/update-status/5
        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("update-status/{shopId}")]
        public IActionResult UpdateShopStatus(int shopId, [FromQuery] string statusName)
        {
            bool result = false;

            if (statusName == "active")
            {
                int? statusId =  _configDataService.GetStatusShopIdByStatusShopName("active");
                result = _shopService.UpdateStatusShopIdByShopId(shopId, statusId.Value);

                int? roleId = _configDataService.GetRoleIdByRoleName("sale");
                _userService.UpdateRoleUserByShopId(shopId, roleId.Value);
            }
            else if (statusName == "reject")
            {
                int? statusId = _configDataService.GetStatusShopIdByStatusShopName("reject");
                result = _shopService.UpdateStatusShopIdByShopId(shopId, statusId.Value);
            }
            else
            {
                return BadRequest(new { message = "Trạng thái không hợp lệ. Chỉ chấp nhận active hoặc reject." });
            }

            if (result)
            {
                return Ok(new { message = "Cập nhật trạng thái thành công." });
            }
            else
            {
                return NotFound(new { message = "Không tìm thấy cửa hàng hoặc lỗi khi cập nhật." });
            }
        }

        [HttpGet("shop-id/{productId}")]
        public IActionResult GetShopIdByProductId(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }

            return Ok(new
            {
                productId = product.ProductId,
                shopId = product.ShopId
            });
        }

    }
}
