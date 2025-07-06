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
    public class ConfigDataRepository : IConfigDataRepository
    {
        private readonly ConfigDataDAO _configDataDao;

        public ConfigDataRepository()
        {
            _configDataDao = new ConfigDataDAO();
        }

        public int? GetStatusShopIdByStatusShopName(string statusName)
        {
            return _configDataDao.GetStatusShopIdByStatusShopName(statusName);
        }

        public int? GetStatusProductIdByStatusProductName(string statusName)
        {
            return _configDataDao.GetStatusProductIdByStatusProductName(statusName);
        }

        public int? GetStatusOrderIdByStatusOrderName(string statusName)
        {
            return _configDataDao.GetStatusOrderIdByStatusOrderName(statusName) ;
        }

        public int? GetRoleIdByRoleName(string roleName)
        {
            return _configDataDao.GetRoleIdByRoleName(roleName);
        }

        public List<ProductSize> GetProductSizes()
        {
            return _configDataDao.GetProductSizes();
        }

        public bool CheckStatusProductExist(string productStatusName)
        {
            return _configDataDao.CheckStatusProductExist(productStatusName);
        }

        public List<StatusOrder> GetStatusOrders()
        {
            return _configDataDao.GetStatusOrders();
        }
    }
}
