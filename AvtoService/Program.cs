using AvtoService.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using System.Net.Http.Headers;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ���������� MudBlazor
builder.Services.AddMudServices();

// ���������� Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ����������� AuthService � HttpClient � BaseAddress
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpClient("GlobalClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7292/"); // ���������, ��� ���� ��������� � ������ ������ API
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// ���������� Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();

// ����������� CustomAuthStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// ��������� �����������
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

// ���������� ��������������
builder.Services.AddAuthentication();

// ��������� CORS
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

// ������������� CORS
app.UseCors("AllowAll");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAntiforgery();

// ��������� ��������� ��� Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// ������������� �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

app.Run();