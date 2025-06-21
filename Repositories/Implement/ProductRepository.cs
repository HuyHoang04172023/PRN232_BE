using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects;
using Repositories.Interface;

namespace Repositories.Implement
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAO _productDao;

        public ProductRepository()
        {
            _productDao = new ProductDAO();
        }
    }
}
