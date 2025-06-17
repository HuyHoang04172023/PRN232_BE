using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Interface
{
    public interface IShopRepository
    {
        List<Shop> GetShops();
        List<Shop> GetShopsByStatusName(string statusName);
    }
}
