using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using AvtoService.Data;
using AvtoService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AvtoService.Data.Requests;

namespace AvtoService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AvtoServiceContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AvtoServiceContext context, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Login == request.Login))
                {
                    return BadRequest("Логин уже используется");
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                _logger.LogInformation("Длина хэша пароля: {Length}", passwordHash.Length);

                var user = new User
                {
                    Login = request.Login,
                    Password = passwordHash, 
                    Role = "user",
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                return Ok(new AuthResponse
                {
                    Token = token,
                    Id = user.Id,
                    Login = user.Login,
                    Role = user.Role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя");
                return StatusCode(500, "Произошла ошибка на сервере");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Попытка входа: Login = {Login}", request.Login);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login);
                if (user == null)
                {
                    _logger.LogWarning("Не найден пользователь с Login: {Login}", request.Login);
                    return Unauthorized("Пользователь не найден");
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
                if (!isPasswordValid)
                {
                    _logger.LogWarning("Неверный пароль для пользователя: {Login}", request.Login);
                    return Unauthorized("Неверный пароль");
                }

                var token = GenerateJwtToken(user);

                var response = new LoginResponse
                {
                    Token = token,
                    Id = user.Id,
                    Login = user.Login,
                    Role = user.Role ?? "user"
                };

                _logger.LogInformation("Успешный вход: {Login}", user.Login);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при входе пользователя");
                return StatusCode(500, "Произошла ошибка на сервере");
            }
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Data.Requests.RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Login == request.Login))
            {
                return BadRequest("Логин уже используется");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            _logger.LogInformation("Хеш пароля при регистрации админа: {PasswordHash}", passwordHash);

            var user = new User
            {
                Login = request.Login,
                Password = passwordHash,
                Role = "admin",
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return Ok(new AuthResponse
            {
                Token = token,
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Role = user.Role
            });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim("Password", user.Password), 
                new Claim(ClaimTypes.Role, user.Role ?? "user")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}