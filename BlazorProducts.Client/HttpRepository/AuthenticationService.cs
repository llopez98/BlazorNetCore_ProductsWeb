using Blazored.LocalStorage;
using BlazorProducts.Client.AuthProviders;
using Entities.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BlazorProducts.Client.HttpRepository
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(HttpClient client, AuthenticationStateProvider authenticationState, ILocalStorageService localStorage)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
            _authStateProvider = authenticationState;
            _localStorage = localStorage;
        }

        public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthenticationDto)
        {
            var content = JsonSerializer.Serialize(userForAuthenticationDto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var authResult = await _client.PostAsync("accounts/login", bodyContent);

            var authContent = await authResult.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent,_options);

            if (!authResult.IsSuccessStatusCode)
                return result;

            //se añade el token al local storage
            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
            //llama el metodo para notificar al usuario
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            //setea el token para las request
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return new AuthResponseDto { 
                IsAuthSuccessful = true
            };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            var tokenDto = JsonSerializer.Serialize(new RefreshTokenDto { Token = token, RefreshToken = refreshToken });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");

            var refreshResult = await _client.PostAsync("token/refresh", bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(refreshContent, _options);

            if (!refreshResult.IsSuccessStatusCode)
                throw new ApplicationException("Something went wrong during the refresh token action");

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return result.Token;
        }

        public async Task<RegistrationResponseDto> RegistrerUser(UserForRegistrationDto userForRegistrationDto)
        {
            var content = JsonSerializer.Serialize(userForRegistrationDto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var registrationResult = await _client.PostAsync("accounts/registration", bodyContent);
            var registrationContent = await registrationResult.Content.ReadAsStringAsync();

            if (!registrationResult.IsSuccessStatusCode) { 
                var result = JsonSerializer.Deserialize<RegistrationResponseDto>(registrationContent, _options);

                return result;
            }

            return new RegistrationResponseDto { IsSuccessfulRegistration = true };
        }
    }
}
