using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ProductDAO
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        public ProductDAO()
        {
            _context = new Prn222BeverageWebsiteProjectContext();
        }

        public Product CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product? GetProductByProductId(int productId)
        {
            return _context.Products
                .Include(p => p.ProductVariants)
                .Include(p => p.Shop)
                .Include(p => p.StatusProduct)
                .FirstOrDefault(p => p.ProductId == productId);
        }

        public List<int> GetDuplicatedSizeIds(List<ProductVariant> variants)
        {
            return variants
                .GroupBy(v => v.ProductSizeId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
        }

        public bool DeleteProduct(Product product)
        {
            try
            {
                _context.ProductVariants.RemoveRange(product.ProductVariants);
                _context.Products.Remove(product);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateStatusProductIdByProductId(int productId, int statusProductId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                return false;
            }

            product.StatusProductId = statusProductId;

            try
            {
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
