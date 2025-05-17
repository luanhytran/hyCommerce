using System.Web;
using DotNetCore.CAP;
using hyCommerce.Application.DTOs;
using hyCommerce.Common.Constants;
using hyCommerce.Common.Event;
using hyCommerce.Domain.Common;
using hyCommerce.Domain.Entities;
using hyCommerce.Infrastructure.Persistence;

namespace hyCommerce.Application.Services;

public interface IIdentityService
{
    Task<Result<AuthResult>> Login(LoginDto loginDto);

    Task<Result<string>> RegisterUser(string baseUrl, RegisterDto registerDto);

    Task<Result> ConfirmEmail(string userId, string token);
    
    Task<Result> UpdateUser(string currentUserId, string targetUserId, UpdateUserDto updateUserDto, bool isAdmin);
}

public class IdentityService(ApplicationUserManager userManager, ITokenService tokenService, ICapPublisher capPublisher) : IIdentityService
{
    public async Task<Result<AuthResult>> Login(LoginDto loginDto)
    {
        try
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
                return Result<AuthResult>.Failure(message: "Invalid credentials");

            if (!user.EmailConfirmed)
                return Result<AuthResult>.Failure(message: "Email not confirmed");

            var authResult = await tokenService.CreateTokenAsync(user);
        
            return Result<AuthResult>.Success(authResult);
        }
        catch (Exception ex)
        {
            return Result<AuthResult>.Failure(message: $"Error login user: {ex.Message}");
        }
    }

    public async Task<Result<string>> RegisterUser(string baseUrl, RegisterDto registerDto)
    {
        try
        {
            var user = new User { Email = registerDto.Email, UserName = registerDto.Email };
            
            var createUserResult = await userManager.CreateAsync(user, registerDto.Password);
            
            if (!createUserResult.Succeeded)
            {
                var errors = string.Join("; ", createUserResult.Errors
                    .Select(e => e.Description));

                return Result<string>.Failure(errors);
            }
            
            await userManager.AddToRoleAsync(user, "Member");

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            
            var confirmationLink = string.Format(AppConstants.EMAIL_CONFIRMATION_URL, baseUrl, user.Id, encodedToken);

            await capPublisher.PublishAsync(nameof(UserCreatedEvent), new UserCreatedEvent
            {
                Email = user.Email,
                UserDisplayName = user.UserName,
                ReturnUrl = confirmationLink
            });
            
            return Result<string>.Success(message: "Registration successful. Please check your email to confirm your account.");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(message: $"Error registering user: {ex.Message}");
        }
    }

    public async Task<Result> ConfirmEmail(string userId, string token)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure("User not found");

            var result = await userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded ? Result.Success("Email confirmed successfully") 
                : Result.Failure("Invalid token or email confirmation failed");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error confirming email: {ex.Message}");
        }
    }

    public async Task<Result> UpdateUser(string currentUserId, string targetUserId, UpdateUserDto updateUserDto, bool isAdmin)
    {
        try
        {
            if (!isAdmin && currentUserId != targetUserId)
                return Result.Failure("You are not authorized to update this user");
            
            var user = await userManager.FindByIdAsync(targetUserId);

            if (user == null)
                return Result.Failure("User not found");

            user.UserName = updateUserDto.UserName;

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded
                ? Result.Success("User updated successfully")
                : Result.Failure($"User update failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error updating user: {ex.Message}");
        }
    }
}