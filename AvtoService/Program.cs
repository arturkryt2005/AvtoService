using AvtoService.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using System.Net.Http.Headers;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Добавление MudBlazor
builder.Services.AddMudServices();

// Добавление Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Регистрация AuthService и HttpClient с BaseAddress
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpClient("GlobalClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7292/"); // Убедитесь, что порт совпадает с портом вашего API
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// Добавление Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Регистрация CustomAuthStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Настройка авторизации
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireAssertion(context =>
    {
        var user = context.User;
        return user.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "admin");
    }));
    options.AddPolicy("UserOnly", policy => policy.RequireAssertion(context =>
    {
        var user = context.User;
        return user.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "user");
    }));
});

// Добавление аутентификации
builder.Services.AddAuthentication();

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Использование CORS
app.UseCors("AllowAll");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAntiforgery();

// Настройка маршрутов для Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Использование аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.Run();