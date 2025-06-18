using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IConfigDataRepository
    {
        int? GetStatusShopIdByStatusShopName(string statusName);
        int? GetStatusProductIdByStatusProductName(string statusName);
        int? GetStatusOrderIdByStatusOrderName(string statusName);
        int? GetRoleIdByRoleName(string roleName);
    }
}
