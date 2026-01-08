using HospitalOPD.Api.Data;
using HospitalOPD.Api.Helpers;
using HospitalOPD.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static HospitalOPD.Api.DTO.UserManagement;
using Microsoft.EntityFrameworkCore; 
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;


namespace HospitalOPD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HospitalDBContext _context;
        private readonly IConfiguration _config;

        public UserController(HospitalDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == dto.Username))
                return BadRequest(new ApiResponse { Success = false, Msg = "User already exists" });

            using var hmac = SHA256.Create();
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));

            var user = new Users
            {
                UserName = dto.Username,
                PasswordHash = passwordHash,
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse { Success = true, Msg = "User registered", Data = null });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == dto.Username);
            if (user == null)
                return Unauthorized(new ApiResponse { Success = false, Msg = "Invalid credentials" });

            using var hmac = SHA256.Create();
            var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));

            if (hash != user.PasswordHash)
                return Unauthorized(new ApiResponse { Success = false, Msg = "Invalid credentials" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(5),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new ApiResponse
            {
                Success = true,
                Msg = "Login successful",
                Data = tokenHandler.WriteToken(token)
            });
        }
    }

}
