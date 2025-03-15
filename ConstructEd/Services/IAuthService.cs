using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Identity;


namespace ConstructEd.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<SignInResult> LoginUserAsync(LoginViewModel model);
        Task<IdentityResult> AddRoleAsync(RoleViewModel model);
        Task LogoutAsync();
    }
}
