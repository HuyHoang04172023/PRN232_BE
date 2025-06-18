using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Services.Interface
{
    public interface IShopService
    {
        List<Shop> GetShops();
        List<Shop> GetShopsByStatusName(string statusName);
        Shop CreateShop(Shop shop);
        Shop? GetShopByUserId(int id);
        Shop? GetShopByShopId(int shopId);
        bool UpdateShop(int shopId, Shop shopUpdate);
        bool DeleteShop(int shopId);
        bool UpdateStatusShopIdByShopId(int shopId, int statusShopId);
    }
}
