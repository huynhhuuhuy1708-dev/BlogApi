using BlogAI.Data;
using BlogAI.Helpers;
using BlogAI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BlogAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwt;

        public AuthController(AppDbContext context, JwtHelper jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if(await _context.Users.AnyAsync(u => u.Username == user.Username))
                return BadRequest("Username already exists");

            user.PasswordHash = ComputeHash(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if(dbUser == null || dbUser.PasswordHash != ComputeHash(user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _jwt.GenerateToken(dbUser);
            return Ok(new { token });
        }

        private string ComputeHash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
