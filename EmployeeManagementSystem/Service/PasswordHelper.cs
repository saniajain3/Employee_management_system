using EmployeeManagementSystem.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EmployeeManagementSystem.Service
{
    public class PasswordHelper
    {
        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher<User>();

            return hasher.HashPassword(null, password);
            
        }
        public bool VerifyPassword(string hashedPassword, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success? true: false;
        }
    }
}
