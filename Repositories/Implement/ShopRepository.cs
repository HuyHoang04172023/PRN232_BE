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

        public Shop? GetShopById(int id)
        {
            return _shopDao.GetShopById(id);
        }
    }
}
