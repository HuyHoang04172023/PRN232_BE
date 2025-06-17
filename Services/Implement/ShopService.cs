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
    }
}
