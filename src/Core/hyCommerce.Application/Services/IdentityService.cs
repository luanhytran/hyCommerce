using System.ComponentModel.DataAnnotations;
using System.Web;
using DotNetCore.CAP;
using hyCommerce.Application.DTOs;
using hyCommerce.Common.Constants;
using hyCommerce.Common.Event;
using hyCommerce.Domain.Entities;
using hyCommerce.Extensions.Exceptions;
using hyCommerce.Infrastructure.Persistence;

namespace hyCommerce.Application.Services;

public interface IIdentityService
{
    Task<AuthResult> Login(LoginDto loginDto);

    Task<string> RegisterUser(string baseUrl, RegisterDto registerDto);

    Task<string> ConfirmEmail(string userId, string token);
    
    Task<string> UpdateUser(string currentUserId, string targetUserId, UpdateUserDto updateUserDto, bool isAdmin);

    Task<string> RequestResetPassword(string email, string baseUrl);
    
    Task<string> ResetPassword(ResetPasswordDto resetPasswordDto);
}

public class IdentityService(ApplicationUserManager userManager, ITokenService tokenService, ICapPublisher capPublisher) : IIdentityService
{
    public async Task<AuthResult> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.UserName);

        if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            throw new ValidationException("Invalid credentials");

        if (!user.EmailConfirmed)
            throw new ValidationException("Email not confirmed");
        
        return await tokenService.CreateTokenAsync(user);
    }

    public async Task<string> RegisterUser(string baseUrl, RegisterDto registerDto)
    {
        var user = new User { Email = registerDto.Email, UserName = registerDto.Email };
        
        var createUserResult = await userManager.CreateAsync(user, registerDto.Password);
        
        if (!createUserResult.Succeeded)
        {
            var errors = string.Join("; ", createUserResult.Errors
                .Select(e => e.Description));

            throw new ValidationException(errors);
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
        
        return "Registration successful. Please check your email to confirm your account.";
    }

    public async Task<string> ConfirmEmail(string userId, string token)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
            throw new NotFoundException("User not found");

        var result = await userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded ? "Email confirmed successfully"
            : throw new ValidationException("Invalid token or email confirmation failed");
    }

    public async Task<string> UpdateUser(string currentUserId, string targetUserId, UpdateUserDto updateUserDto, bool isAdmin)
    {
        if (!isAdmin && currentUserId != targetUserId)
            throw new InvalidOperationException("You are not authorized to update this user");
        
        var user = await userManager.FindByIdAsync(targetUserId);

        if (user == null)
            throw new NotFoundException("User not found");

        user.UserName = updateUserDto.UserName;

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded
            ? "User updated successfully"
            : throw new ValidationException( 
                $"User update failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");
    }

    public async Task<string> RequestResetPassword(string email, string baseUrl)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        if (user == null)
            throw new NotFoundException("User not found");
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        
        var resetLink = string.Format(AppConstants.EMAIL_CONFIRMATION_URL, baseUrl, user.Id, encodedToken);

        await capPublisher.PublishAsync(nameof(ResetPasswordEvent), new ResetPasswordEvent()
        {
            Email = user.Email,
            UserDisplayName = user.UserName,
            ReturnUrl = resetLink
        });
        
        return "Reset password link sent to your email";
    }

    public async Task<string> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await userManager.FindByIdAsync(resetPasswordDto.UserId);
        
        if (user == null)
            throw new NotFoundException("User not found");
        
        var decodedToken = HttpUtility.UrlDecode(resetPasswordDto.Token);
        
        var result = await userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDto.NewPassword);
        
        return result.Succeeded ? "Password reset successfully"
            : throw new ValidationException( 
                $"Password reset failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");      
    }
}