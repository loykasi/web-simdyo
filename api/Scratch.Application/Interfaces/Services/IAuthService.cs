using Scratch.Application.Models.Requests;
using Scratch.Application.Results;
using Scratch.Application.Models.Responses;

namespace Scratch.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result> RequestLoginAsync(string email);
        Task<Result<LoginResponse>> LoginWithOtpAsync(LoginOtpRequest request);
        Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest);
        Task<Result> ConfirmEmail(ConfirmEmailRequest request);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);
        Task<Result> RefreshTokenAsync(string? refreshToken);
        Task<Result> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        Task<Result> ResetPassword(ResetPasswordRequest resetPasswordRequest);
        Task<Result<AuthUserResponse>> GetCurrentUser();
        Task<Result> LogOut();
        Task<Result> ChangePassword(ChangePasswordRequest changePasswordRequest);
        Task<Result<AccountDetailResponse>> GetProfileDetail(string userName);
    }
}
