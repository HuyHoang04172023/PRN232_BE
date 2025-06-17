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
    }
}
