using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects;
using Repositories.Interface;

namespace Repositories.Implement
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO _userDao;

        public UserRepository()
        {
            _userDao = new UserDAO();
        }

        public bool UpdateRoleUserByShopId(int shopId, int roleId)
        {
            return _userDao.UpdateRoleUserByShopId(shopId, roleId);
        }
    }
}
