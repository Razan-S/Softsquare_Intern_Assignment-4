using Microsoft.AspNetCore.Mvc;
using dotnet_training.Models;
using dotnet_training.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace dotnet_training.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login/admin")]
        public IActionResult LoginAdmin([FromBody] User loginRequest)
        {
            var token = GenerateJwtToken(loginRequest.Username, "admin");

            return Ok(new { Token = token });
        }

        [HttpPost("login/user")]
        public IActionResult Login([FromBody] User loginRequest) 
        {
            var token = GenerateJwtToken(loginRequest.Username, "user");

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string userName, string role)
        {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    }
}
