using ConstructionSite.AppHost.ServiceDefaults;
using ConstructionSite.Services.Activity.API;
using ConstructionSite.Services.Activity.Application.Common.Behaviors;
using ConstructionSite.Services.Activity.Application.Features.ActivityTypes;
using ConstructionSite.Services.Activity.Application.Security;
using ConstructionSite.Services.Activity.Domain.Contracts;
using ConstructionSite.Services.Activity.Infrastructure.Data;
using ConstructionSite.Services.Activity.Infrastructure.Repositories;
using ConstructionSite.Shared;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

#region [ Database ]

builder.AddSqlServerDbContext<ActivityDbContext>(
    "activity-db",
    configureDbContextOptions: opt =>
    {
        opt.UseSqlServer(sqlOptions =>
        {
            opt.UseLazyLoadingProxies();
            sqlOptions.MigrationsAssembly("ConstructionSite.Services.Activity.Infrastructure");
        });
    }
);

#endregion

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllActivityTypesQueryHandler).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

#region [ OpenAPI / Swashbuckle ]

builder.Services.AddOpenApi("v1");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Construction Site Activity API",
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

List<string> _claims = [.. Permissions.ActivityTypes.Metrics, ..Permissions.Activities.Metrics];

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

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

#region [ Endpoints ]

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Construction Site Activity API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

#endregion

#region [ Database Migrations ]

using var scope = app.Services.CreateScope();
var _db = scope.ServiceProvider.GetRequiredService<ActivityDbContext>();

if (_db.Database.GetPendingMigrations().Any())
{
    _db.Database.Migrate();
}

#endregion

app.Run();
