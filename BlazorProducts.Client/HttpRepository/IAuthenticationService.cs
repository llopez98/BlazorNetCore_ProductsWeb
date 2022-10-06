using Entities.DTO;

namespace BlazorProducts.Client.HttpRepository
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDto> RegistrerUser(UserForRegistrationDto userForRegistrationDto);
        Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthenticationDto);
        Task Logout();
        Task<string> RefreshToken();
    }
}
