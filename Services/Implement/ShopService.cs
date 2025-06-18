using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implement
{
    public class ShopService : IShopService
    {
        private readonly ShopRepository _shopRepository;

        public ShopService()
        {
            _shopRepository = new ShopRepository();
        }

        public List<Shop> GetShops()
        {
            return _shopRepository.GetShops();
        }

        public List<Shop> GetShopsByStatusName(string statusName)
        {
            return _shopRepository.GetShopsByStatusName(statusName);
        }

        public Shop CreateShop(Shop shop)
        {
            return _shopRepository.CreateShop(shop);
        }

        public Shop? GetShopByUserId(int id)
        {
            return _shopRepository.GetShopByUserId(id);
        }

        public Shop? GetShopByShopId(int shopId)
        {
            return _shopRepository.GetShopByShopId(shopId);
        }

        public bool UpdateShop(int shopId, Shop shopUpdate)
        {
            return _shopRepository.UpdateShop(shopId, shopUpdate);
        }

        public bool DeleteShop(int shopId)
        {
            return _shopRepository.DeleteShop(shopId);
        }
    }
}
