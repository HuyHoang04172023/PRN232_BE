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

namespace Project_PRN232_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        private readonly ShopService _shopService;

        public ShopsController(Prn222BeverageWebsiteProjectContext context)
        {
            _context = context;
            _shopService = new ShopService();
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

        // GET: api/Shops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> GetShop(int id)
        {
            var shop = await _context.Shops.FindAsync(id);

            if (shop == null)
            {
                return NotFound();
            }

            return shop;
        }

        // PUT: api/Shops/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShop(int id, Shop shop)
        {
            if (id != shop.ShopId)
            {
                return BadRequest();
            }

            _context.Entry(shop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopExists(id))
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

        // POST: api/Shops
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Shop>> PostShop(Shop shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShop", new { id = shop.ShopId }, shop);
        }

        // DELETE: api/Shops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShopExists(int id)
        {
            return _context.Shops.Any(e => e.ShopId == id);
        }
    }
}
