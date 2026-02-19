using Application.Authorization;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models.Emails;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace Application.Services
{
    public class AuthService
    (
        IAuthTokenService authTokenProcessor,
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IEmailService emailSender,
        IConfiguration configuration,
        ICurrentUserService currentUserService,
        ICookieService cookieService
    ) : IAuthService
    {
        public async Task<Result> RequestLoginAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result.NotFound
                (
                    new Error("Account.NotFound", $"Account not found with email {email}")
                );
            }

            bool isBanned = await unitOfWork.UserBanRepository.GetBanStatus(user.Id);
            if (isBanned)
            {
                return Result.Failure<LoginResponse>
                (
                    new Error("Auth.AccountBanned", $"Account has been banned.")
                );
            }

            await userManager.UpdateSecurityStampAsync(user);
            var token = await userManager.GenerateUserTokenAsync
            (
                user,
                "PasswordlessLoginTotpProvider",
                "passwordless-auth"
            );
            SendOTPEmail(user, token);

            return Result.Success();
        }

        private void SendOTPEmail(User user, string token)
        {
            EmailMessage emailMessage = new()
            {
                ToName = user.Username,
                ToEmail = user.Email!,
                Subject = "Login code to Simdyo",
                TemplateName = "OTP.html",
                Placeholders =
                [
                    new("OTP", token)
                ]
            };

            emailSender.Send(emailMessage);
        }

        public async Task<Result<LoginResponse>> LoginWithOtpAsync(LoginOtpRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure<LoginResponse>
                (
                    new Error("Auth.InvalidCredentials", $"Invalid email or password.")
                );
            }

            var isValid = await userManager.VerifyUserTokenAsync
            (
                user,
                "PasswordlessLoginTotpProvider",
                "passwordless-auth",
                request.Code
            );
            if (!isValid)
            {
                return Result.Failure<LoginResponse>
                (
                    new Error("Auth.InvalidCredentials", $"Invalid email or password.")
                );
            }

            if (user.LastUsedToken == request.Code)
            {
                return Result.Failure<LoginResponse>
                (
                    new Error("Auth.InvalidCredentials", $"Invalid email or password.")
                );
            }

            user.LastUsedToken = request.Code;
            await userManager.UpdateAsync(user);

            bool isBanned = await unitOfWork.UserBanRepository.GetBanStatus(user.Id);
            if (isBanned)
            {
                return Result.Failure<LoginResponse>
                (
                    new Error("Auth.AccountBanned", $"Account has been banned.")
                );
            }

            // confirm email on login
            user.EmailConfirmed = true;

            // generate access and refresh token
            var (jwtToken, expiration) = await authTokenProcessor.GenerateJwtToken(user, isUseOTP: true);
            var refreshToken = authTokenProcessor.GenerateRefreshToken(user);

            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);

            cookieService.SetToken("ACCESS_TOKEN", jwtToken, expiration);
            cookieService.SetToken("REFRESH_TOKEN", refreshToken.Token, refreshToken.RefreshTokenExpriresAtUTC);

            var permissions = await unitOfWork.UserRespository.GetUserPermissionsAsync(user);

            return Result.Success
            (
                new LoginResponse
                (
                    Username: user.UserName!,
                    Email: user.Email!,
                    ExpiresAt: expiration.ToString("o"),
                    IsUseOTP: true,
                    Permissions: permissions
                )
            );
        }

        public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            bool isUserExists = await userManager.FindByNameAsync(registerRequest.Username) != null;
            if (isUserExists)
            {
                return Result.Conflict<RegisterResponse>
                (
                    new Error("User.DuplicateUsername", $"User with username: {registerRequest.Username} already exists")
                );
            }

            isUserExists = await userManager.FindByEmailAsync(registerRequest.Email) != null;
            if (isUserExists)
            {
                return Result.Conflict<RegisterResponse>
                (
                    new Error("User.DuplicateEmail", $"User with email: {registerRequest.Email} already exists")
                );
            }

            User user = User.Create(registerRequest.Email, registerRequest.Username);

            var result = await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, Roles.Member);

            if (!result.Succeeded)
            {
                return Result.BadRequest<RegisterResponse>
                (
                    [.. result.Errors.Select(e => new Error(e.Code, e.Description))]
                );
            }

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
            var user = await userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null
                || string.IsNullOrWhiteSpace(user.PasswordHash)
                || !await userManager.IsEmailConfirmedAsync(user)
                || !await userManager.CheckPasswordAsync(user, loginRequest.Password)
                )
            {
                return Result.Failure<LoginResponse>
                (
                    new Error("Auth.InvalidCredentials", $"Invalid email or password.")
                );
            }

            bool isBanned = await unitOfWork.UserBanRepository.GetBanStatus(user.Id);
            if (isBanned)
            {
                return Result.Failure<LoginResponse>
                (
                    new Error("Auth.AccountBanned", $"Account has been banned.")
                );
            }

            var (jwtToken, expiration) = await authTokenProcessor.GenerateJwtToken(user, isUseOTP: false);
            var refreshToken = authTokenProcessor.GenerateRefreshToken(user);

            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);

            var permissions = await unitOfWork.UserRespository.GetUserPermissionsAsync(user);

            cookieService.SetToken("ACCESS_TOKEN", jwtToken, expiration);
            cookieService.SetToken("REFRESH_TOKEN", refreshToken.Token, refreshToken.RefreshTokenExpriresAtUTC);

            return Result.Success
            (
                new LoginResponse
                (
                    Username: user.UserName!,
                    Email: user.Email!,
                    ExpiresAt: expiration.ToString("o"),
                    IsUseOTP: false,
                    Permissions: permissions
                )
            );
        }

        public async Task<Result> RefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Result.NotFound(new Error("RefreshToken.Missing", "Refresh token is missing."));
            }

            var user = await unitOfWork.UserRespository.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                RevokeClientAccessCookie();
                return Result.UnAuthorized(new Error("RefreshToken.Invalid", "Refresh token is invalid."));
            }

            var refreshTokenModel = authTokenProcessor.GetRefreshTokenByUser(user, refreshToken);
            if (refreshTokenModel == null)
            {
                RevokeClientAccessCookie();
                return Result.UnAuthorized(new Error("RefreshToken.Expired", "Refresh token is expired."));
            }

            if (refreshTokenModel.RefreshTokenExpriresAtUTC < DateTime.UtcNow)
            {
                RevokeClientAccessCookie();
                await authTokenProcessor.RevokeToken(user, refreshTokenModel.Token);
                return Result.UnAuthorized(new Error("RefreshToken.Expired", "Refresh token is expired."));
            }

            bool isBanned = await unitOfWork.UserBanRepository.GetBanStatus(user.Id);
            if (isBanned)
            {
                RevokeClientAccessCookie();
                return Result.UnAuthorized(new Error("RefreshToken.Expired", "Refresh token is expired."));
            }

            var (jwtToken, expirationDateInUtc) = await authTokenProcessor.GenerateJwtToken(user);
            refreshTokenModel = authTokenProcessor.GenerateRefreshToken(user, refreshToken);

            await unitOfWork.SaveChangesAsync();

            cookieService.SetToken("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
            cookieService.SetToken("REFRESH_TOKEN", refreshTokenModel.Token, refreshTokenModel.RefreshTokenExpriresAtUTC);

            return Result.Success();
        }

        private void RevokeClientAccessCookie()
        {
            cookieService.Delete("ACCESS_TOKEN");
            cookieService.Delete("REFRESH_TOKEN");
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

            await SendPasswordResetEmail(user);

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

        private async Task SendConfirmationEmail(User user)
        {
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            string baseUrl = configuration.GetSection("URL")["Web"]!;
            string url = $"{baseUrl}/confirm-email?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";

            EmailMessage emailMessage = new()
            {
                ToName = user.Username,
                ToEmail = user.Email!,
                Subject = "Account Verification",
                TemplateName = "ConfirmationMail.html",
                Placeholders =
                [
                    new("url", url)
                ]
            };

            emailSender.Send(emailMessage);
        }

        private async Task SendPasswordResetEmail(User user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            string baseUrl = configuration.GetSection("URL")["Web"]!;
            string url = $"{baseUrl}/reset-password?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";
            
            EmailMessage emailMessage = new()
            {
                ToName = user.Username,
                ToEmail = user.Email!,
                Subject = "Reset Password",
                TemplateName = "ResetPasswordMail.html",
                Placeholders =
                [
                    new("url", url)
                ]
            };

            emailSender.Send(emailMessage);
        }

        public async Task<Result<AuthUserResponse>> GetCurrentUser()
        {
            string userID = currentUserService.GetUserID();
            
            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return Result.NotFound<AuthUserResponse>
                (
                    new Error("Profile.NotFound", "Invalid token")
                );
            }

            bool isBanned = await unitOfWork.UserBanRepository.GetBanStatus(user.Id);
            var permissions = await unitOfWork.UserRespository.GetUserPermissionsAsync(user);

            return Result.Success
            (
                new AuthUserResponse(user.UserName!, user.Email!, isBanned, permissions)
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

            await authTokenProcessor.RevokeToken(user, refreshToken);
            //user.RefreshToken = string.Empty;
            //user.RefreshTokenExpriresAtUTC = null;
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
                return Result.NotFound<AuthUserResponse>
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
