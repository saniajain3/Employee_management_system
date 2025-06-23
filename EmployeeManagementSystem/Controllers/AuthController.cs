using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Data.IRepository<User, long> userRepo;
        private readonly IConfiguration configuration;
        private readonly Data.IRepository<Employee, long> empRepo;

        public AuthController(IRepository<User, long> userRepo, IConfiguration configuration, IRepository<Employee, long> empRepo)
        {
            this.userRepo = userRepo;
            this.configuration = configuration;
            this.empRepo = empRepo;
            // Constructor logic can be added here if needed
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto model)
        {
            var user = (await userRepo.GetAll(x => x.Email == model.Email)).FirstOrDefault();

            if (user == null)
            {
                return new BadRequestObjectResult(new { message = "User not found" });
            }

            var PasswordHelper = new PasswordHelper();


            if (!PasswordHelper.VerifyPassword(user.Password, model.Password))
            {
                return new BadRequestObjectResult(new { message = "Invalid password" });
            }
            var token = GenerateToken(user.Email, user.Role);
            return Ok(new AuthTokenDto()
            {
                Id = user.Id,
                Email = user.Email,
                Token = token, // Generate a new token for the session
                Role = user.Role,
            });
        }
        private string GenerateToken(string Email, string Role)
        {

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,Email),
                new Claim(ClaimTypes.Role,Role)
            };
            var token = new JwtSecurityToken(claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDto model)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (email == null)
                return BadRequest(new { message = "User email not found in claims." });

            var user = (await userRepo.GetAll(x => x.Email == email)).FirstOrDefault();
            if (user == null)
                return NotFound(new { message = "User not found." });

            // ✅ Only update allowed fields
            if (!string.IsNullOrWhiteSpace(model.Username))
                user.Username = model.Username;

            if (!string.IsNullOrWhiteSpace(model.ProfileImage))
                user.ProfileImage = model.ProfileImage;

            userRepo.Update(user);
            await userRepo.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully." });
        }


        [Authorize]
        [HttpGet("get-profile")]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            var user = (await userRepo.GetAll(x => x.Email == email)).FirstOrDefault();
                       
            var employee = (await empRepo.GetAll(x => x.UserId == user.Id)).FirstOrDefault();

            return Ok(new ProfileDto
            {
                Username = user?.Username,
                Name = employee?.Name,
                Email = user.Email,
                Phone = employee?.Phone,
                ProfileImage = user.ProfileImage
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            User user = null;

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                user = (await userRepo.GetAll(u => u.Email == dto.Email)).FirstOrDefault();
            }

            if (user == null)
                return NotFound("User not found using the provided ID or Email.");

            var passwordHelper = new PasswordHelper();
            user.Password = passwordHelper.HashPassword(dto.NewPassword);

            userRepo.Update(user);
            await userRepo.SaveChangesAsync();

            return Ok("Password updated successfully.");
        }



    }
}
