﻿using AutoMapper;
using ConstructEd.Models;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Identity;
using ConstructEd.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMapper _mapper;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
    {
        var user = _mapper.Map<ApplicationUser>(model);
        user.UserName = model.Email; 
        user.CreatedAt = DateTime.UtcNow;

        var result = await _userManager.CreateAsync(user, model.Password);

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