﻿using BlazorProducts.Client.Features;
using Entities.Models;
using Entities.RequestFeatures;

namespace BlazorProducts.Client.HttpRepository
{
    public interface IProductHttpRepository
    {
        Task<PagingResponse<Product>> GetProducts( ProductParameters productParameters);

        Task CreateProduct(Product product);

        Task<string> UploadProductImage(MultipartFormDataContent content);

        Task<Product> GetProduct(string id);

        Task UpdateProduct(Product product);

        Task DeleteProduct(Guid id);

    }
}
