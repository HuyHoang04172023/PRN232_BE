﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implement
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }

        public Product CreateProduct(Product product)
        {
            return _productRepository.CreateProduct(product);
        }

        public bool DeleteProduct(Product product)
        {
            return _productRepository.DeleteProduct(product);
        }

        public List<int> GetDuplicatedSizeIds(List<ProductVariant> variants)
        {
            return _productRepository.GetDuplicatedSizeIds(variants);
        }

        public Product? GetProductByProductId(int productId)
        {
            return _productRepository.GetProductByProductId(productId);
        }

        public List<Product> GetProductsByShopId(int shopId)
        {
            return _productRepository.GetProductsByShopId(shopId);
        }

        public List<Product> GetProductsByStatusName(string statusName)
        {
            return _productRepository.GetProductsByStatusName(statusName);
        }

        public bool UpdateStatusProductIdByProductId(int productId, int statusProductId)
        {
            return _productRepository.UpdateStatusProductIdByProductId(productId, statusProductId);
        }
    }
}
