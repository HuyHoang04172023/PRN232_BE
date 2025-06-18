using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using BusinessObjects.ModelsDTO.Shop;
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

        public Shop CreateShop(Shop shop)
        {
            _context.Shops.Add(shop);
            _context.SaveChanges();
            return shop;
        }

        public Shop? GetShopByUserId(int userId)
        {
            return _context.Shops
                .Include(s => s.StatusShop)
                .FirstOrDefault(s => s.CreatedBy == userId);
        }

        public Shop? GetShopByShopId(int shopId)
        {
            return _context.Shops
                .Include(s => s.StatusShop)
                .FirstOrDefault(s => s.ShopId == shopId);
        }

        public bool UpdateShop(int shopId, Shop shopUpdate)
        {
            Shop? shop = _context.Shops.FirstOrDefault(s => s.ShopId == shopId);

            if (shop == null)
                return false;

            shop.ShopName = shopUpdate.ShopName;
            shop.ShopAddress = shopUpdate.ShopAddress;
            shop.ShopImage = shopUpdate.ShopImage;
            shop.ShopDescription = shopUpdate.ShopDescription;
            shop.StatusShopId = shopUpdate.StatusShopId;

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteShop(int shopId)
        {
            var shop = _context.Shops.FirstOrDefault(s => s.ShopId == shopId);

            if (shop == null)
                return false;

            _context.Shops.Remove(shop);

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateStatusShopIdByShopId(int shopId, int statusShopId)
        {
            var shop = _context.Shops.FirstOrDefault(s => s.ShopId == shopId);

            if (shop == null)
            {
                return false;
            }

            shop.StatusShopId = statusShopId;

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
