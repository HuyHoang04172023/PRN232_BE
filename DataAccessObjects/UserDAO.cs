using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class UserDAO
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        public UserDAO()
        {
            _context = new Prn222BeverageWebsiteProjectContext();
        }

        public bool UpdateRoleUserByShopId(int shopId, int roleId)
        {
            try
            {
                var shop = _context.Shops
                                .Include(s => s.CreatedByNavigation)
                                .FirstOrDefault(s => s.ShopId == shopId);

                if (shop == null || shop.CreatedByNavigation == null)
                {
                    return false;
                }

                var user = shop.CreatedByNavigation;

                if (user.RoleId == roleId)
                {
                    return true;
                }

                user.RoleId = roleId;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
