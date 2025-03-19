using AutoMapper;
using ConstructEd.Models;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Identity;
using ConstructEd.Services;
using ConstructEd.Repositories;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IInstructorRepository _instructorRepository;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        IInstructorRepository instructorRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _instructorRepository = instructorRepository;
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
    {
        var user = _mapper.Map<ApplicationUser>(model);
        user.CreatedAt = DateTime.UtcNow;

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, Role.User.ToString());
        }
        return result;
    }
    public async Task<IdentityResult> RegisterInstructorAsync(InstructorViewModel model)
    {
        var user = _mapper.Map<ApplicationUser>(model);
        user.CreatedAt = DateTime.UtcNow;

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, Role.Instructor.ToString());

            var instructor = new Instructor
            {
                UserId = user.Id,
                Bio = model.Bio,
                ProfilePicture = model.ProfilePicture,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _instructorRepository.InsertAsync(instructor);
        }
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

    public async Task<IdentityResult> AddRoleAsync(RoleViewModel model)
    {
        return await _roleManager.CreateAsync(new IdentityRole(model.UserRole.ToString()));
    }

}