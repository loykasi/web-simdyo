using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Scratch.Application.Abstracts;
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
        IUserRespository userRespository,
        UserManager<User> userManager,
        IEmailSender emailSender,
        IConfiguration configuration,
        ICurrentUserService currentUserService,
        ITokenService tokenService
    ) : IAccountService
    {
        private readonly IAuthTokenProcessor _authTokenProcessor = authTokenProcessor;
        private readonly IUserRespository _userRespository = userRespository;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IConfiguration _configuration = configuration;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            bool isUserExists = await _userManager.FindByNameAsync(registerRequest.UserName) != null;
            if (isUserExists)
            {
                return Result.Conflict<RegisterResponse>
                (
                    new Error("User.UsernameDuplicate", $"User with username: {registerRequest.UserName} already exists")
                );
            }

            isUserExists = await _userManager.FindByEmailAsync(registerRequest.Email) != null;
            if (isUserExists)
            {
                return Result.Conflict<RegisterResponse>
                (
                    new Error("User.EmailDuplicate", $"User with email: {registerRequest.Email} already exists")
                );
            }

            User user = User.Create(registerRequest.Email, registerRequest.UserName);

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                return Result.BadRequest<RegisterResponse>
                (
                    [.. result.Errors.Select(e => new Error(e.Code, e.Description))]
                );
            }
            
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
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

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.NotFound(new Error("User.notFound", "User not found"));
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
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
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null
                || !await _userManager.IsEmailConfirmedAsync(user)
                || !await _userManager.CheckPasswordAsync(user, loginRequest.Password)
                )
            {
                return Result.Failure<LoginResponse>
                (
                    new Error("Login.ValidationFailed", $"Invalid email or password.")
                );
            }

            var (jwtToken, expiration) = _authTokenProcessor.GenerateJwtToken(user);
            var refreshToken = _authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationAtUTC = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpriresAtUTC = refreshTokenExpirationAtUTC;

            await _userManager.UpdateAsync(user);

            //_authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expiration);
            //_authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshTokenExpirationAtUTC);

            _tokenService.SetToken("ACCESS_TOKEN", jwtToken, expiration);
            _tokenService.SetToken("REFRESH_TOKEN", user.RefreshToken, refreshTokenExpirationAtUTC);

            return Result.Success
            (
                new LoginResponse(user.UserName!, user.Email!)
            );
        }

        public async Task RefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new RefreshTokenException("Refresh token is missing.");
            }

            var user = await _userRespository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                throw new RefreshTokenException("Invalid refresh token");
            }

            if (user.RefreshTokenExpriresAtUTC < DateTime.UtcNow)
            {
                throw new RefreshTokenException("Refresh token is expired.");
            }

            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
            var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpriresAtUTC = refreshTokenExpirationDateInUtc;

            await _userManager.UpdateAsync(user);

            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshTokenExpirationDateInUtc);
        }

        public async Task<Result> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordRequest.Email);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("User.NotFound", "InvalidEmail")
                );
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await SendPasswordResetEmail(user, token);

            return Result.Success();
        }

        public async Task<Result> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            // valid request dto first

            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("ResetPassword.NotFound", "InvalidEmail")
                );
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.Password);

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
            string baseUrl = _configuration.GetSection("URLOptions")["Web"]!;
            string url = $"{baseUrl}/confirm-email?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";
            string body = $"Click link to verify your account: <a href=\"{url}\" target=\"_blank\" >Click here</a>";

            await _emailSender.Send(user.UserName!, user.Email!, "Account Verification", body);
        }

        private async Task SendPasswordResetEmail(User user, string token)
        {
            string baseUrl = _configuration.GetSection("URLOptions")["Web"]!;
            string url = $"{baseUrl}/reset-password?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";
            string body = $"Click link to reset your password: <a href=\"{url}\" target=\"_blank\" >Click here</a>" +
                $"\ntoken: {token}";

            await _emailSender.Send(user.UserName!, user.Email!, "Reset Password", body);
        }

        public async Task<Result<ProfileResponse>> GetProfile()
        {
            string userID = _currentUserService.GetUserID();
            
            var user = await _userManager.FindByIdAsync(userID);
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
            return Result.Success();
        }
    }
}
