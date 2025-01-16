using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.Authentications;
using LibrarySystem.Services.Authentications.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibrarySystem.Services.Authentications.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger, IUnitOfWork unitOfWork, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _context = context;
        }



        public async Task<ServiceResponse<TokensResponse>> LoginWithJwt(Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new ServiceResponse<TokensResponse>() { Entities = null, Success = false, Message = "Error in Email or Password!" };

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = GetToken(userClaims); // access token


            var tokens = await GetJwtTokn(user);


            return new ServiceResponse<TokensResponse>()
            {
                Entities = new TokensResponse()
                {
                    AccessToken = tokens.Entities.AccessToken.Token,
                    AccessTokenExpireOn = tokens.Entities.AccessToken.ExpireTokenDate,
                },
                Success = tokens.Success,
                Message = tokens.Message
            };
        }

        public async Task<ServiceResponse<LoginResponse>> RenewAccessToken(LoginResponse tokens)
        {
            try
            {
                var accessToken = tokens.AccessToken;
                var principal = GetClaimsPrincipal(accessToken.Token);

                var user = await _userManager.FindByNameAsync(principal.Identity.Name);

                var response = await GetJwtTokn(user);
                return response;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LoginResponse>() { Message = ex.Message };
            }
        }



        //// Private Methods.

        private JwtSecurityToken GetToken(List<Claim> userClaim)
        {
            var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInDays"], out int tokenValidityInDays);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(tokenValidityInDays),
                claims: userClaim,
                signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }


        private async Task<ServiceResponse<LoginResponse>> GetJwtTokn(ApplicationUser user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = GetToken(userClaims); // access token

            await _userManager.UpdateAsync(user);

            return new ServiceResponse<LoginResponse>
            {
                Entities = new LoginResponse()
                {
                    AccessToken = new JwtToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        // ExpireTokenDate = jwtToken.ValidTo
                        ExpireTokenDate = jwtToken.ValidTo
                    },
                },
                Success = true,
                Message = "Tokens"
            };
        }


        private ClaimsPrincipal GetClaimsPrincipal(string accessToken)
        {
            var tokenValidateionParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidateionParameters, out SecurityToken
                 securityToken);

            return principal;
        }
    }
}

