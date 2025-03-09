using AvtoService.Data.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity.Data;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AvtoService.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            Console.WriteLine($"Отправка запроса: Email={request.Email}, Password={request.Password}");
            Console.WriteLine($"Сериализованный JSON: {System.Text.Json.JsonSerializer.Serialize(request)}");

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Invalid login attempt.");
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            await _localStorage.SetItemAsStringAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("userId", result.Id);
            await _localStorage.SetItemAsStringAsync("username", result.Login);
            await _localStorage.SetItemAsStringAsync("email", result.Password);
            await _localStorage.SetItemAsStringAsync("role", result.Role);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
            return result;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> GetToken()
        {
            return await _localStorage.GetItemAsStringAsync("authToken");
        }

        public async Task<AuthResponse?> Register(RegisterRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                Console.WriteLine($"Отправляемые данные: {json}");

                var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка регистрации: {errorContent}");
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при регистрации: {ex.Message}");
                throw;
            }
        }

        public async Task<AuthResponse?> RegisterAdmin(RegisterRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                Console.WriteLine($"Отправляемые данные для регистрации админа: {json}");


                var response = await _httpClient.PostAsJsonAsync("api/auth/register-admin", request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка регистрации админа: {errorContent}");
                    return null;
                }


                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при регистрации админа: {ex.Message}");
                throw;
            }
        }
    }
}
