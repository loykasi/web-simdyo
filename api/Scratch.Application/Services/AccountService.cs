using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Repositories;
using Scratch.Domain.Entities;
using Scratch.Domain.Exceptions;
using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;
using System.Web;

namespace Scratch.Application.Services
{
    public class AccountService
    (
        IAuthTokenProcessor authTokenProcessor,
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IEmailSender emailSender,
        IConfiguration configuration,
        ICurrentUserService currentUserService,
        ICookieService cookieService
    ) : IAccountService
    {
        public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            bool isUserExists = await userManager.FindByNameAsync(registerRequest.UserName) != null;
            if (isUserExists)
            {
                return Result.Conflict<RegisterResponse>
                (
                    new Error("User.UsernameDuplicate", $"User with username: {registerRequest.UserName} already exists")
                );
            }

            isUserExists = await userManager.FindByEmailAsync(registerRequest.Email) != null;
            if (isUserExists)
            {
                return Result.Conflict<RegisterResponse>
                (
                    new Error("User.EmailDuplicate", $"User with email: {registerRequest.Email} already exists")
                );
            }

            User user = User.Create(registerRequest.Email, registerRequest.UserName);

            var result = await userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                return Result.BadRequest<RegisterResponse>
                (
                    [.. result.Errors.Select(e => new Error(e.Code, e.Description))]
                );
            }
            
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendConfirmationEmail(user, token);

            return Result.Success(
                new RegisterResponse("Registration successful. Please check your email to verify your account.")
            );
        }

        public async Task<Result> ConfirmEmail(ConfirmEmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Email))
            {
                return Result.NotFound(new Error("Token.Notfound", "Token is missing"));
            }

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.NotFound(new Error("User.notFound", "User not found"));
            }

            var result = await userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded)
            {
                return Result.BadRequest
                (
                    [.. result.Errors.Select(e => new Error(e.Code, e.Description))]
                );
            }

            return Result.Success();
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var user = await userManager.FindByNameAsync(loginRequest.UserName);

            if (user == null
                || !await userManager.IsEmailConfirmedAsync(user)
                || !await userManager.CheckPasswordAsync(user, loginRequest.Password)
                )
            {
                return Result.Failure<LoginResponse>
                (
                    new Error("Login.ValidationFailed", $"Invalid email or password.")
                );
            }

            var (jwtToken, expiration) = authTokenProcessor.GenerateJwtToken(user);
            var refreshToken = authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationAtUTC = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpriresAtUTC = refreshTokenExpirationAtUTC;

            await userManager.UpdateAsync(user);

            cookieService.SetToken("ACCESS_TOKEN", jwtToken, expiration);
            cookieService.SetToken("REFRESH_TOKEN", user.RefreshToken, refreshTokenExpirationAtUTC);

            return Result.Success
            (
                new LoginResponse(user.UserName!, user.Email!, expiration.ToString("o"))
            );
        }

        public async Task<Result> RefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Result.NotFound(new Error("RefreshTokenMissing", "Refresh token is missing."));
            }

            var user = await unitOfWork.UserRespository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                return Result.UnAuthorized(new Error("InvalidToken", "Refresh token is invalid."));
            }

            if (user.RefreshTokenExpriresAtUTC < DateTime.UtcNow)
            {
                return Result.UnAuthorized(new Error("ExpiredToken", "Refresh token is expired."));
            }

            var (jwtToken, expirationDateInUtc) = authTokenProcessor.GenerateJwtToken(user);
            var refreshTokenValue = authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpriresAtUTC = refreshTokenExpirationDateInUtc;

            await userManager.UpdateAsync(user);

            authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
            authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshTokenExpirationDateInUtc);

            return Result.Success();
        }

        public async Task<Result> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var user = await userManager.FindByEmailAsync(forgotPasswordRequest.Email);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("User.NotFound", "InvalidEmail")
                );
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            await SendPasswordResetEmail(user, token);

            return Result.Success();
        }

        public async Task<Result> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            // valid request dto first

            var user = await userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("ResetPassword.NotFound", "InvalidEmail")
                );
            }

            var result = await userManager.ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.Password);

            if (!result.Succeeded)
            {
                return Result.NotFound
                (
                    [.. result.Errors.Select(e => new Error(e.Code, e.Description))]
                );
            }

            return Result.Success();
        }

        private async Task SendConfirmationEmail(User user, string token)
        {
            string baseUrl = configuration.GetSection("URLOptions")["Web"]!;
            string url = $"{baseUrl}/confirm-email?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";
            string body = $"Click link to verify your account: <a href=\"{url}\" target=\"_blank\" >Click here</a>";

            await emailSender.Send(user.UserName!, user.Email!, "Account Verification", body);
        }

        private async Task SendPasswordResetEmail(User user, string token)
        {
            string baseUrl = configuration.GetSection("URLOptions")["Web"]!;
            string url = $"{baseUrl}/reset-password?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";
            string body = $"Click link to reset your password: <a href=\"{url}\" target=\"_blank\" >Click here</a>" +
                $"\ntoken: {token}";

            await emailSender.Send(user.UserName!, user.Email!, "Reset Password", body);
        }

        public async Task<Result<ProfileResponse>> GetProfile()
        {
            string userID = currentUserService.GetUserID();
            
            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound<ProfileResponse>
                (
                    new Error("Profile.NotFound", "Invalid token")
                );
            }

            return Result.Success
            (
                new ProfileResponse(user.UserName!, user.Email!)
            );
        }

        public async Task<Result> LogOut()
        {
            string refreshToken = cookieService.Get("REFRESH_TOKEN");
            var user = await unitOfWork.UserRespository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                throw new RefreshTokenException("Invalid refresh token");
            }

            user.RefreshToken = string.Empty;
            user.RefreshTokenExpriresAtUTC = null;
            await userManager.UpdateAsync(user);

            cookieService.Delete("ACCESS_TOKEN");
            cookieService.Delete("REFRESH_TOKEN");

            return Result.Success();
        }

        public async Task<Result> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            string userID = currentUserService.GetUserID();

            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound<ProfileResponse>
                (
                    new Error("Profile.NotFound", "Invalid token")
                );
            }

            var result = await userManager.ChangePasswordAsync
                                        (
                                            user,
                                            changePasswordRequest.CurrentPassword,
                                            changePasswordRequest.NewPassword
                                        );

            if (!result.Succeeded)
            {
                return Result.NotFound
                (
                    [.. result.Errors.Select(e => new Error(e.Code, e.Description))]
                );
            }

            return Result.Success();
        }

        public async Task<Result<AccountDetailResponse>> GetProfileDetail(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Result.NotFound<AccountDetailResponse>
                (
                    new Error("Profile.NotFound", "Invalid token")
                );
            }

            int totalProject = await unitOfWork.ProjectRepository.GetUserProjectCount(user.Id);

            return Result.Success
            (
                new AccountDetailResponse(user.UserName!, user.Email!, totalProject)
            );
        }
    }
}
