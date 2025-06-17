using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class ShopDAO
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        public ShopDAO()
        {
            _context = new Prn222BeverageWebsiteProjectContext();
        }

        public List<Shop> GetShops()
        {
            return _context.Shops
                .Include(s => s.StatusShop)
                .Include(s => s.CreatedByNavigation)
                .Include(s => s.ApprovedByNavigation)
                .ToList();
        }

        public List<Shop> GetShopsByStatusName(string statusName) 
        { 
            return _context.Shops
                .Include(s => s.StatusShop)
                .Include(s => s.CreatedByNavigation)
                .Include(s => s.ApprovedByNavigation)
                .Where(s => s.StatusShop != null && s.StatusShop.StatusShopName == statusName)
                .ToList();
        }
    }
}
