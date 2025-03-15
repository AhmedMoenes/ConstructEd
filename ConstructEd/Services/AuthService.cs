using AutoMapper;
using ConstructEd.Models;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Identity;
using ConstructEd.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
    {
        var user = _mapper.Map<ApplicationUser>(model);
        user.CreatedAt = DateTime.UtcNow;

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            const string defaultRole = "User";
            var roleExists = await _roleManager.RoleExistsAsync(defaultRole);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(defaultRole));
            }

            await _userManager.AddToRoleAsync(user, defaultRole);
        }

        return result;
    }
    public async Task<IdentityResult> AddRoleAsync(RoleViewModel model)
    {
        IdentityRole role = new IdentityRole();
        role.Name = model.RoleName;
        IdentityResult result = await _roleManager.CreateAsync(role);
        return result;
    }
    public async Task<SignInResult> LoginUserAsync(LoginViewModel model)
    {
        return await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: false);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}