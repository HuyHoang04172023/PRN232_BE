using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IProductRepository
    {
        Product CreateProduct(Product product);
        Product? GetProductByProductId(int productId);
        List<int> GetDuplicatedSizeIds(List<ProductVariant> variants);
        bool DeleteProduct(Product product);
        bool UpdateStatusProductIdByProductId(int productId, int statusProductId);
    }
}
