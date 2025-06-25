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
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAO _productDao;

        public ProductRepository()
        {
            _productDao = new ProductDAO();
        }

        public Product CreateProduct(Product product)
        {
            return _productDao.CreateProduct(product);
        }

        public bool DeleteProduct(Product product)
        {
            return _productDao.DeleteProduct(product);
        }

        public List<int> GetDuplicatedSizeIds(List<ProductVariant> variants)
        {
            return _productDao.GetDuplicatedSizeIds(variants);
        }

        public Product? GetProductByProductId(int productId)
        {
            return _productDao.GetProductByProductId(productId);
        }

        public bool UpdateStatusProductIdByProductId(int productId, int statusProductId)
        {
            return _productDao.UpdateStatusProductIdByProductId(productId, statusProductId);
        }
    }
}
