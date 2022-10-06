using BlazorProducts.Client.HttpRepository;
using BlazorProducts.Client.Shared;
using Entities.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Pages
{
    public partial class UpdateProduct
    {
        private Product _product;
        private SuccessNotification _notification;

        [Inject]
        IProductHttpRepository ProductRepo { get; set; }
        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync() {
            _product = await ProductRepo.GetProduct(Id);
            bool flag = true;
        }

        private async Task Update() {
            await ProductRepo.UpdateProduct(_product);
            _notification.Show();
        }

        private void AssignImageUrl(string imgUrl) => _product.ImageUrl = imgUrl;
    }
}
