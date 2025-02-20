using ConstructionSite.Web.Components;
using ConstructionSite.Web;
using ConstructionSite.Web.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;
using Radzen;
using ConstructionSite.Shared;
using ConstructionSite.Web.Client;
using ConstructionSite.AppHost.ServiceDefaults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddHttpClient<AuthenticationApiClient>(client =>
{
    client.BaseAddress = new("https+http://authentication-api");
});

builder.Services.AddHttpClient<ActivityApiClient>(client =>
{
    client.BaseAddress = new("https+http://activity-api");
});

builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https+http://authentication-api");
});


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddRadzenComponents();
builder.Services.AddScoped<DialogService>();
builder.Services.AddMudServices();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CustomScheme";
    options.DefaultChallengeScheme = "CustomScheme";
})
.AddScheme<CustomAuthOptions, CustomAuthenticationHandler>("CustomScheme", options => { });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("admin", "role"));
    });
    List<string> _claims = [.. Permissions.ActivityTypes.Metrics, .. Permissions.Activities.Metrics];

    foreach (var item in _claims)
    {
        options.AddPolicy(item, builder =>
        {
            builder.RequireAuthenticatedUser()
                   .RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "role" && c.Value == "admin") ||
                        context.User.HasClaim(c => c.Type == "API.Permission" && c.Value == item));
        });
    }
});

// Services required by authentication and authorisation
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
