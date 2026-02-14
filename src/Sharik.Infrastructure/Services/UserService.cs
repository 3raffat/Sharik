using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Auth;
using Sharik.Domain.Common.Results;
using Sharik.Domain.User.Enums;
using System.Text;

namespace Sharik.Infrastructure.Auth
{
    public sealed class UserService
        (ILogger<UserService> _logger , UserManager<AppUser> _manager , ITokenProvider _token , IEmailService _emailService) : IUserService
    {
        public async Task<Result<Success>> ConfirmEmailAsync(string userId , string token , CancellationToken ct)
        {
            var user = await GetUserByIdAsync(userId);

            if (user is null)
            {
                _logger.LogWarning("Email confirmation attempt failed: User with ID {UserId} not found." , userId);
                return AuthErrors.UserNotFound;
            }

            if (user.EmailConfirmed)
            {
                _logger.LogInformation("Email confirmation attempt: Email for user with ID {UserId} is already confirmed." , userId);
                return AuthErrors.EmailAlreadyConfirmed;    
            }

            var decodedToken = DecodedToken(token);

            var result = await _manager.ConfirmEmailAsync(user , decodedToken);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Email confirmation attempt failed: User with ID {UserId} not found." , userId);
                return AuthErrors.UserNotFound;
            }

            return Result.Success;
        }

        public async Task<Result<LoginUserDto>> LoginAsync(string email , string password , CancellationToken ct)
        {

            var user = await _manager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogWarning("Login attempt failed: User with email {Email} not found." , email);
                return AuthErrors.InvalidEmailOrPassword;
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Login attempt failed: Email {Email} is not confirmed." , email);
                return AuthErrors.EmailNotConfirmed;
            }

            var validPassword = await _manager.CheckPasswordAsync(user , password);

            if (!validPassword)
            {
                _logger.LogWarning("Login attempt failed: Invalid password for user with email {Email}." , email);
                return AuthErrors.InvalidEmailOrPassword;
            }

            var userInfo = await GetUserInfoAsync(user);

            var tokenResult = await _token.GenerateJwtTokenAsync(userInfo , ct);

            if (tokenResult.IsFailure)
            {
                _logger.LogError("Token generation failed for user with email {Email}." , email);
                return tokenResult.Errors;
            }


            return new LoginUserDto(email , tokenResult.Value);
        }

        public async Task<Result<Success>> RegisterAsync(string username , string email , string password , CancellationToken ct)
        {
            var existingUsername = await _manager.FindByNameAsync(username);

            if (existingUsername is not null)
            {
                _logger.LogWarning("Registration attempt failed: Username {Username} is already taken." , username);
                return AuthErrors.UsernameAlreadyExists;
            }

            var existingEmail = await _manager.FindByEmailAsync(email);

            if (existingEmail is not null)
            {
                _logger.LogWarning("Registration attempt failed: Email {Email} is already registered." , email);
                return AuthErrors.InvalidEmail;
            }

            var newUser = AppUser.Create(username , email);

            if (newUser.IsFailure)
                return newUser.Errors;

            var createResult = await _manager.CreateAsync(newUser.Value , password);

            if (!createResult.Succeeded)
                return createResult.Errors.Select(e => Error.Failure(e.Code , e.Description)).ToList();

            var addToRoleResult = await _manager.AddToRoleAsync(newUser.Value , nameof(Role.User));

            if (!addToRoleResult.Succeeded)
            {
                _logger.LogError("Failed to assign role to user with email {Email}. Rolling back user creation." , email);
                await _manager.DeleteAsync(newUser.Value);
                return addToRoleResult.Errors.Select(e => Error.Failure(e.Code , e.Description)).ToList();
            }

            _logger.LogInformation("User with email {Email} successfully registered." , email);


            var token = await _manager.GenerateEmailConfirmationTokenAsync(newUser.Value);

            var encodedToken = EncodedToken(token);

            var confirmationLink = $"https://localhost:5000/auth/v1/confirm-email?userId={newUser.Value.Id}&token={encodedToken}";

            var emailResult = await _emailService.SendConfirmationEmailAsync(email , username , confirmationLink , ct);

            if (emailResult.IsFailure)
            {
                _logger.LogError("Failed to send confirmation email to {Email}. Rolling back user creation." , email);
                await _manager.DeleteAsync(newUser.Value);
                return emailResult.Errors;
            }

            return Result.Success;
        }

        private async Task<AppUserDto> GetUserInfoAsync(AppUser user)
        {

            var roles = await _manager.GetRolesAsync(user);

            var claims = await _manager.GetClaimsAsync(user);


            return new AppUserDto(user.GetIdString() , user.Email! , roles , claims);

        }

        private async Task<AppUser?> GetUserByIdAsync(string userId)
        {
            return await _manager.FindByIdAsync(userId);
        }

        private string DecodedToken(string token)
        {
            var decodedToken = Encoding.UTF8.GetString(
                                             WebEncoders.Base64UrlDecode(token));

            return decodedToken;
        }
        private string EncodedToken(string token)
        {
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }
    }
}