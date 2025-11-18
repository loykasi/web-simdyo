using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Scratch.API.Extensions;
using Scratch.API.Handlers;
using Scratch.Infrastructure;
using Scratch.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString"));
});

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization(builder.Configuration);
builder.Services.SetupCors();
builder.Services.SetupServices();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("API");
    });

    await app.SeedRolesAndPermissions();
    await app.SeedAdmin();
}

app.UseCors("AllowOrigins");
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
