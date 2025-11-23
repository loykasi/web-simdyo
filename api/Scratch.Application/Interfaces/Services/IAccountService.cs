using Scratch.Domain.Requests;
using Scratch.Domain.Responses;
using Scratch.Domain.Results;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IAccountService
    {
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
