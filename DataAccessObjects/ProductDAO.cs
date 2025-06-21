using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace DataAccessObjects
{
    public class ProductDAO
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        public ProductDAO()
        {
            _context = new Prn222BeverageWebsiteProjectContext();
        }
    }
}
