using BusinessObjects.Models;
using BusinessObjects.ModelsDTO.ConfigData;
using BusinessObjects.ModelsDTO.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Implement;
using Services.Interface;

namespace Project_PRN232_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigDataController : ControllerBase
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        private readonly ConfigDataService _configDataService;

        public ConfigDataController()
        {
            _context = new Prn222BeverageWebsiteProjectContext();
            _configDataService = new ConfigDataService();
        }

        [HttpGet("sizes")]
        public IActionResult GetAllProductSizes()
        {
            try
            {
                List<ProductSize> productSizes = _configDataService.GetProductSizes();

                var response = productSizes.Select(s => new ProductSizeResponse
                {
                    ProductSizeId = s.ProductSizeId,
                    ProductSizeName = s.ProductSizeName
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách kích cỡ sản phẩm", error = ex.Message });
            }
        }

        [HttpGet("status-orders")]
        public IActionResult GetAllStatusOrders()
        {
            try
            {
                List<StatusOrder> statusOrders = _configDataService.GetStatusOrders();

                var response = statusOrders.Select(s => new StatusOrderResponse
                {
                    StatusOrderId = s.StatusOrderId,
                    StatusOrderName = s.StatusOrderName
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy trạng thái đơn hàng", error = ex.Message });
            }
        }
    }
}
