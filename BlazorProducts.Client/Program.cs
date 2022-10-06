using Blazored.LocalStorage;
using BlazorProducts.Client;
using BlazorProducts.Client.AuthProviders;
using BlazorProducts.Client.HttpRepository;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tewr.Blazor.FileReader;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5027/api/") }.EnableIntercept(sp));

builder.Services.AddScoped<IProductHttpRepository, ProductHttpRepository>();

builder.Services.AddFileReaderService(o => o.UseWasmSharedBuffer = true);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<RefreshTokenService>();

builder.Services.AddScoped<HttpInterceptorService>();

/*builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
}
    .EnableIntercept(sp));*/

builder.Services.AddHttpClientInterceptor();

await builder.Build().RunAsync();
