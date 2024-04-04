using Drone.Services.AuthApi.Data;
using Drone.Services.AuthApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Drone.Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ApplicationDbContext context;
        private IConfiguration _configuration;
        public AuthController(ApplicationDbContext _context, IConfiguration configuration)
        {
            _configuration = configuration;
            context = _context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginUser model)
        {
            try
            {
                context.Users.Add(model);
                context.SaveChanges();
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to register user: " + ex.Message);
            }
        }
        private string GenerateToken(LoginUser users)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(1),
            signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUser model)
        {
             IActionResult response = null;
            var user = context.Users.FirstOrDefault(u => u.username == model.username && u.password == model.password);
            if (user == null)
            {
                return Unauthorized();
            }
            else if (user != null)
            {
                var token = GenerateToken(user);
                response = Ok(new { token = token });
            }
            return response;
            
            }



        }
    }

