﻿using BlazorProducts.Client.HttpRepository;
using BlazorProducts.Client.Shared;
using Entities.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Pages
{
    public partial class CreateProduct
    {
        private Product _product = new Product();
        private SuccessNotification _notification;

        [Inject]
        public IProductHttpRepository ProductRepo { get; set; }

        private async void Create() {
            await ProductRepo.CreateProduct(_product);
            _notification.Show();
        }

        private void AssignImageUrl(string imgUrl) => _product.ImageUrl = imgUrl;
    }
}
