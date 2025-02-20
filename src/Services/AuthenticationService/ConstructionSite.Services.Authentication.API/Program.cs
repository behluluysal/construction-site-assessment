using ConstructionSite.AppHost.ServiceDefaults;
using ConstructionSite.Services.Authentication.API;
using ConstructionSite.Services.Authentication.Application.Common.Behaviors;
using ConstructionSite.Services.Authentication.Application.Common.Interfaces;
using ConstructionSite.Services.Authentication.Application.Features.Authentication;
using ConstructionSite.Services.Authentication.Application.Security;
using ConstructionSite.Services.Authentication.Application.Services;
using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Contracts;
using ConstructionSite.Services.Authentication.Domain.Entities;
using ConstructionSite.Services.Authentication.Infrastructure.Data;
using ConstructionSite.Services.Authentication.Infrastructure.Repositories;
using ConstructionSite.Services.Authentication.Infrastructure.Security;
using ConstructionSite.Shared;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region [ Logging ]

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
    logging.ParseStateValues = true;
});

#endregion

#region [ JWT Settings ]

JwtSettings jwtSettings = new();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

#endregion

#region [ Database ]

builder.AddSqlServerDbContext<IdentityDatabaseContext>(
    "authentication-db",
    configureDbContextOptions: opt =>
    {
        opt.UseSqlServer(sqlOptions =>
        {
            opt.UseLazyLoadingProxies();
            sqlOptions.MigrationsAssembly("ConstructionSite.Services.Authentication.Infrastructure");
        });
    }
);

#endregion

#region [ Additional Services ]

// Repositories
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDataProtection();
builder.Services.AddLocalization(options => options.ResourcesPath = "");

builder.Services
    .AddIdentityCore<ApplicationUser>(identityOptions =>
    {
        identityOptions.Password.RequiredLength = 8;
    })
    .AddRoles<ApplicationRole>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddEntityFrameworkStores<IdentityDatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.AddServiceDefaults();

#endregion

#region [ OpenAPI / Swashbuckle ]

builder.Services.AddOpenApi("v1");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Construction Site Authentication API",
        Version = "v1"
    });
});

#endregion

#region [ Jwt/Permissions ]

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("admin");
    });

List<string> _claims = [.. Permissions.Users.Metrics];
foreach (var permission in _claims)
{
    builder.Services.AddAuthorizationBuilder()
        .AddPolicy(permission, policy =>
        {
            policy.RequireAuthenticatedUser()
                  .AddRequirements(new PermissionRequirement(permission, "API.Permission"));
        });
}


builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

#endregion

#region [ Localization ]

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    string[] supportedCultures = ["en-US", "tr-TR"];
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

    options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
    {
        var acceptLanguageHeader = context.Request.Headers.AcceptLanguage.ToString();

        var preferredLanguage = acceptLanguageHeader.Split(',').FirstOrDefault();

        var isValidCulture = supportedCultures.Any(c => string.Equals(c, preferredLanguage, StringComparison.OrdinalIgnoreCase));
        ProviderCultureResult? result = isValidCulture
            ? new ProviderCultureResult(preferredLanguage)
            : new ProviderCultureResult(supportedCultures[0]);

        return Task.FromResult<ProviderCultureResult?>(result);
    }));
});


#endregion

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

#region [ Endpoints ]

app.MapDefaultEndpoints();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Construction Site Authentication API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

#endregion

#region [ Database Migrations ]

using var scope = app.Services.CreateScope();
var _db = scope.ServiceProvider.GetRequiredService<IdentityDatabaseContext>();

if (_db.Database.GetPendingMigrations().Any())
{
    _db.Database.Migrate();
}

#endregion

app.Run();
