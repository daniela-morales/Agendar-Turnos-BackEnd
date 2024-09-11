using GestionTurnosApis.Context;
using GestionTurnosApis.Models;
using GestionTurnosApis.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionTurnosApis.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string Authenticate(string username, string password)
        {
            var user = _context.User.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetUserByName(string username)
        {
            var user = await _context.User
            .Where(u => u.Username == username)
            .Select(u => new User
            {
                IdUser = u.IdUser,
                DocumentNumber = u.DocumentNumber,
                Username = u.Username,
                Password = u.Password
            })
            .FirstOrDefaultAsync();

            return user;
        }
    }
}