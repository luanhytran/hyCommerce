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

    Task<Result> RequestResetPassword(string email, string baseUrl);
    
    Task<Result> ResetPassword(ResetPasswordDto resetPasswordDto);
}

public class IdentityService(ApplicationUserManager userManager, ITokenService tokenService, ICapPublisher capPublisher) : IIdentityService
{
    public async Task<Result<AuthResult>> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.UserName);

        if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            return Result.Failure<AuthResult>(new Error("IdentityErrors.InvalidCredentials","Invalid credentials"));

        if (!user.EmailConfirmed)
            return Result.Failure<AuthResult>(new Error("IdentityErrors.ConfirmEmail","Email not confirmed"));

        var authResult = await tokenService.CreateTokenAsync(user);

        return Result.Success(authResult);
    }

    public async Task<Result<string>> RegisterUser(string baseUrl, RegisterDto registerDto)
    {
        var user = new User { Email = registerDto.Email, UserName = registerDto.Email };
        
        var createUserResult = await userManager.CreateAsync(user, registerDto.Password);
        
        if (!createUserResult.Succeeded)
        {
            var errors = string.Join("; ", createUserResult.Errors
                .Select(e => e.Description));

            return Result<string>.ValidationFailure(new Error("IdentityErrors.Validation",errors));
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
        
        return Result.Success("Registration successful. Please check your email to confirm your account.");
    }

    public async Task<Result> ConfirmEmail(string userId, string token)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
            return new Error("IdentityErrors.UserNotFound","User not found");

        var result = await userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded ? Result.Success("Email confirmed successfully") 
            : new Error("IdentityErrors.ConfirmEmail", "Invalid token or email confirmation failed");
    }

    public async Task<Result> UpdateUser(string currentUserId, string targetUserId, UpdateUserDto updateUserDto, bool isAdmin)
    {
        if (!isAdmin && currentUserId != targetUserId)
            return new Error("IdentityErrors.Authorized", "You are not authorized to update this user");
        
        var user = await userManager.FindByIdAsync(targetUserId);

        if (user == null)
            return new Error("IdentityErrors.UserNotFound", "User not found");

        user.UserName = updateUserDto.UserName;

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success("User updated successfully")
            : new Error("IdentityErrors.UserUpdateFailed", 
                $"User update failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");
    }

    public async Task<Result> RequestResetPassword(string email, string baseUrl)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        if (user == null)
            return new Error("IdentityErrors.UserNotFound","User not found");
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        
        var resetLink = string.Format(AppConstants.EMAIL_CONFIRMATION_URL, baseUrl, user.Id, encodedToken);

        await capPublisher.PublishAsync(nameof(ResetPasswordEvent), new ResetPasswordEvent()
        {
            Email = user.Email,
            UserDisplayName = user.UserName,
            ReturnUrl = resetLink
        });
        
        return Result.Success("Reset password link sent to your email");
    }

    public async Task<Result> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await userManager.FindByIdAsync(resetPasswordDto.UserId);
        
        if (user == null)
            return new Error("IdentityErrors.UserNotFound","User not found");
        
        var decodedToken = HttpUtility.UrlDecode(resetPasswordDto.Token);
        
        var result = await userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDto.NewPassword);
        
        return result.Succeeded ? Result.Success("Password reset successfully")
            : new Error("IdentityErrors.PasswordResetFailed", 
                $"Password reset failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");      
    }
}