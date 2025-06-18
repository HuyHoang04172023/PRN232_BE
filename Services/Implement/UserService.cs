using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Implement;
using Services.Interface;

namespace Services.Implement
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public bool UpdateRoleUserByShopId(int shopId, int roleId)
        {
            return _userRepository.UpdateRoleUserByShopId(shopId, roleId);
        }
    }
}
