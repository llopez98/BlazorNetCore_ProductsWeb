﻿using BLazorProducts.Server.Paging;
using Entities.Models;
using Entities.RequestFeatures;

namespace BLazorProducts.Server.Repository
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetProducts(ProductParameters productParameters);

        Task CreateProduct(Product product);

        Task<Product> GetProduct(Guid id);

        Task UpdateProduct(Product product, Product dbProduct);

        Task DeleteProduct(Product product);
    }
}
