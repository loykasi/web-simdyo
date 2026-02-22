using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace Infrastructure.TokenProviders
{
    public class PasswordlessLoginTotpTokenProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser> where TUser : class
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }

        public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var email = await manager.GetEmailAsync(user);
            return $"PasswordlessLogin:{purpose}:{email}";
        }

        public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            ArgumentNullException.ThrowIfNull(manager);
            byte[] token = await manager.CreateSecurityTokenAsync(user);
            string modifier = await GetUserModifierAsync(purpose, manager, user);

            return TotpService.GenerateCode(token, modifier).ToString("D6", CultureInfo.InvariantCulture);
        }

        public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            ArgumentNullException.ThrowIfNull(user);
            if (!int.TryParse(token, out int code))
            {
                return false;
            }

            byte[] securityToken = await manager.CreateSecurityTokenAsync(user);
            string modifier = await GetUserModifierAsync(purpose, manager, user);

            return securityToken != null && TotpService.ValidateCode(securityToken, code, modifier, 3);
        }
    }
}
