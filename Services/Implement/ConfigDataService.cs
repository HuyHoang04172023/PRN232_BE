using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repositories.Implement;
using Services.Interface;

namespace Services.Implement
{
    public class ConfigDataService : IConfigDataService
    {
        private readonly ConfigDataRepository _configDataRepository;

        public ConfigDataService()
        {
            _configDataRepository = new ConfigDataRepository();
        }

        public int? GetStatusShopIdByStatusShopName(string statusName)
        {
            return _configDataRepository.GetStatusShopIdByStatusShopName(statusName);
        }

        public int? GetStatusProductIdByStatusProductName(string statusName)
        {
            return _configDataRepository.GetStatusProductIdByStatusProductName(statusName);
        }

        public int? GetStatusOrderIdByStatusOrderName(string statusName)
        {
            return _configDataRepository.GetStatusOrderIdByStatusOrderName(statusName);
        }

        public int? GetRoleIdByRoleName(string roleName)
        {
            return _configDataRepository.GetRoleIdByRoleName(roleName);
        }

        public List<ProductSize> GetProductSizes()
        {
            return _configDataRepository.GetProductSizes();
        }
    }
}
