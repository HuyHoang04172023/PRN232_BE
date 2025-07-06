using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Services.Interface
{
    public interface IConfigDataService
    {
        int? GetStatusShopIdByStatusShopName(string statusName);
        int? GetStatusProductIdByStatusProductName(string statusName);
        int? GetStatusOrderIdByStatusOrderName(string statusName);
        int? GetRoleIdByRoleName(string roleName);
        List<ProductSize> GetProductSizes();
        bool CheckStatusProductExist(string productStatusName);
        List<StatusOrder> GetStatusOrders();
    }
}
