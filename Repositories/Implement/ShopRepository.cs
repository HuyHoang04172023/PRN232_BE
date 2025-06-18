using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interface;

namespace Repositories.Implement
{
    public class ShopRepository : IShopRepository
    {
        private readonly ShopDAO _shopDao;

        public ShopRepository()
        {
            _shopDao = new ShopDAO();
        }

        public List<Shop> GetShops()
        {
            return _shopDao.GetShops();
        }

        public List<Shop> GetShopsByStatusName(string statusName)
        {
            return _shopDao.GetShopsByStatusName(statusName);
        }
        public Shop CreateShop(Shop shop)
        {
            return _shopDao.CreateShop(shop);
        }

        public Shop? GetShopByUserId(int id)
        {
            return _shopDao.GetShopByUserId(id);
        }

        public Shop? GetShopByShopId(int shopId)
        {
            return _shopDao.GetShopByShopId(shopId);
        }

        public bool UpdateShop(int shopId, Shop shopUpdate)
        {
            return _shopDao.UpdateShop(shopId, shopUpdate);
        }

        public bool DeleteShop(int shopId)
        {
            return _shopDao.DeleteShop(shopId);
        }

        public bool UpdateStatusShopIdByShopId(int shopId, int statusShopId)
        {
            return _shopDao.UpdateStatusShopIdByShopId(shopId, statusShopId);
        }
    }
}
