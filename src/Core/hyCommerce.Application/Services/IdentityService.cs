using System.Web;
using Azure.Core;
using DotNetCore.CAP;
using hyCommerce.Application.DTOs;
using hyCommerce.Common.Event;
using hyCommerce.Domain.Common;
using hyCommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace hyCommerce.Application.Services;

public interface IIdentityService
{
    Task<Result<AuthResult>> Login(LoginDto loginDto);

    Task<Result> RegisterUser(User user, string? confirmationLink);

    Task<Result> ConfirmEmail(string userId, string token);
}

public class IdentityService(UserManager<User> userManager, ITokenService tokenService, ICapPublisher capPublisher) : IIdentityService
{
    public async Task<Result<AuthResult>> Login(LoginDto loginDto)
    {
        try
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
                return Result<AuthResult>.Failure("Invalid credentials");

            if (!user.EmailConfirmed)
                return Result<AuthResult>.Failure("Email not confirmed");

            var authResult = await tokenService.CreateTokenAsync(user);
        
            return Result<AuthResult>.Success(authResult);
        }
        catch (Exception ex)
        {
            return Result<AuthResult>.Failure($"Error login user: {ex.Message}");
        }
    }

    public async Task<Result> RegisterUser(User user, string confirmationLink)
    {
        try
        {
            await capPublisher.PublishAsync(nameof(UserCreatedEvent), new UserCreatedEvent
            {
                Email = user.Email,
                UserDisplayName = user.UserName,
                ReturnUrl = confirmationLink
            });
            
            return Result.Success("Registration successful. Please check your email to confirm your account.");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error registering user: {ex.Message}");
        }
    }

    public async Task<Result> ConfirmEmail(string userId, string token)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure("User not found");

            var decodedToken = HttpUtility.UrlDecode(token);

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            return result.Succeeded ? Result.Success("Email confirmed successfully") 
                : Result.Failure("Invalid token or email confirmation failed");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error confirming product: {ex.Message}");
        }
    }
}