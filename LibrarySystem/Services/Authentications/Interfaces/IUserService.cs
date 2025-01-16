using LibrarySystem.Models.Authentications;

namespace LibrarySystem.Services.Authentications.Interfaces
{
    public interface IUserService
    {

        Task<ServiceResponse<TokensResponse>> LoginWithJwt(Login model);

        Task<ServiceResponse<LoginResponse>> RenewAccessToken(LoginResponse tokens);
    }
}
