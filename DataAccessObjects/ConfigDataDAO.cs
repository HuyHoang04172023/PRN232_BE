using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace DataAccessObjects
{
    public class ConfigDataDAO
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        public ConfigDataDAO()
        {
            _context = new Prn222BeverageWebsiteProjectContext();
        }

        public int? GetStatusShopIdByStatusShopName(string statusName)
        {
            return _context.StatusShops
                .Where(s => s.StatusShopName == statusName)
                .Select(s => (int?)s.StatusShopId)
                .FirstOrDefault();
        }

        public int? GetStatusProductIdByStatusProductName(string statusName)
        {
            return _context.StatusProducts
                .Where(s => s.StatusProductName == statusName)
                .Select(s => (int?)s.StatusProductId)
                .FirstOrDefault();
        }

        public int? GetStatusOrderIdByStatusOrderName(string statusName)
        {
            return _context.StatusOrders
                .Where(s => s.StatusOrderName == statusName)
                .Select(s => (int?)s.StatusOrderId)
                .FirstOrDefault();
        }

        public int? GetRoleIdByRoleName(string roleName)
        {
            return _context.Roles
                .Where(r => r.RoleName == roleName)
                .Select(r => (int?)r.RoleId)
                .FirstOrDefault();
        }

        public List<ProductSize> GetProductSizes()
        {
            return _context.ProductSizes.ToList();
        }
    }
}
