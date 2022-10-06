using BLazorProducts.Server.Context;
using BLazorProducts.Server.TokenHelpers;
using Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BLazorProducts.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public TokenController(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto) {
            if (refreshTokenDto is null) {
                return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request"});
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            var username = principal.Identity.Name;

            var user = await _userManager.FindByEmailAsync(username);

            if (user == null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });

            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = await _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            user.RefreshToken = _tokenService.GenerateRefresToken();

            await _userManager.UpdateAsync(user);

            return Ok(new AuthResponseDto { Token = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true});
        }
    }
}
