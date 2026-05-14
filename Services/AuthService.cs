using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SOAP.Models;
using SOAP.Repository;

namespace SOAP.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<string> RegisterAsync(User user)
        {
            // Default role
            if (string.IsNullOrEmpty(user.Role))
                user.Role = "User";

            await _userRepository.AddAsync(user);

            return GenerateToken(user);
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || user.PasswordHash != password)
                return null;

            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role) // 🔥 ROLE HERE
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}