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
    }
}
