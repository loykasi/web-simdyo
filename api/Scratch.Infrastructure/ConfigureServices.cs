using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Services;
using Scratch.Application.Services;
using Scratch.Domain.Entities;
using Scratch.Infrastructure.Options;
using Scratch.Infrastructure.Services;
using Scratch.Infrastructure.TokenProviders;
using System.Text;

namespace Scratch.Infrastructure
{
    public static class ConfigureServices
    {
        public static void ConfigureOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection(JwtOptions.JwtOptionKey)
            );
            builder.Services.Configure<EmailOptions>(
                builder.Configuration.GetSection(EmailOptions.EmailOptionsKey)
            );
            builder.Services.Configure<URLOptions>(
                builder.Configuration.GetSection(URLOptions.OptionKey)
            );
            builder.Services.Configure<S3Options>(
                builder.Configuration.GetSection(S3Options.OptionsKey)
            );
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<PasswordlessLoginTotpTokenProvider<User>>("PasswordlessLoginTotpProvider");

            return services;
        }

        public static IServiceCollection AddAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JwtOptionKey).Get<JwtOptions>()
                    ?? throw new ArgumentException(nameof(JwtOptions));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["ACCESS_TOKEN"];
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();
            
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProjectReactionService, ProjectReactionService>();
            services.AddScoped<IProjectCommentService, ProjectCommentService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserBanService, UserBanService>();
            services.AddScoped<IProjectReportService, ProjectReportService>();
            services.AddScoped<IProjectBanService, ProjectBanService>();
            services.AddScoped<IProjectCategoryService, ProjectCategoryService>();

            services.AddScoped<IAuthTokenService, AuthTokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPublicIdService, PublicIdService>();

            services.AddSingleton<IObjectStorageService, ObjectStorageService>();

            return services;
        }

        public static IServiceCollection AddBackgroundJobsServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(configuration.GetConnectionString("DbConnectionString"))
                )
            );

            services.AddHangfireServer();

            return services;
        }
    }
}
