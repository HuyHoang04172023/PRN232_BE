using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IConfigDataRepository
    {
        public int? GetStatusShopIdByStatusShopName(string statusName);
        public int? GetStatusProductIdByStatusProductName(string statusName);
        public int? GetStatusOrderIdByStatusOrderName(string statusName);
    }
}
