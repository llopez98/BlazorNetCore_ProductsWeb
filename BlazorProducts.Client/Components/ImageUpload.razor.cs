using BlazorProducts.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using Tewr.Blazor.FileReader;

namespace BlazorProducts.Client.Components
{
    public partial class ImageUpload
    {
        //private ElementReference _input;

        [Parameter]
        public string ImgUrl { get; set; }
        [Parameter]
        public EventCallback<string> OnChange { get; set; }
        //[Inject]
        //public IFileReaderService FileReaderService { get; set; }
        [Inject]
        public IProductHttpRepository Repository { get; set; }

        private async Task HandleSelected(InputFileChangeEventArgs e) {
            var imageFiles = e.GetMultipleFiles();

            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null)
                {
                    var resizedFile = await imageFile.RequestImageFileAsync("image/png", 300, 500);

                    using (var ms = resizedFile.OpenReadStream(resizedFile.Size))
                    {
                        var content = new MultipartFormDataContent();
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                        content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "image", imageFile.Name);

                        ImgUrl = await Repository.UploadProductImage(content);

                        await OnChange.InvokeAsync(ImgUrl);
                    }
                }
            }
            /*foreach (var file in await FileReaderService.CreateReference(_input).EnumerateFilesAsync()) {
                if (file != null) {
                    var fileInfo = await file.ReadFileInfoAsync();

                    using (var ms = await file.CreateMemoryStreamAsync(4 * 1024)) {
                        var content = new MultipartFormDataContent();
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                        content.Add(new StreamContent(ms, Convert.ToInt32(ms.Length)), "image", fileInfo.Name);

                        ImgUrl = await Repository.UploadProductImage(content);

                        await OnChange.InvokeAsync(ImgUrl);
                    }
                }
            }*/
        }
    }
}
