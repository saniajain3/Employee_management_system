namespace EmployeeManagementSystem.Entity
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; } // Added Email for user identification
        public string Password { get; set; }
        public string Role { get; set; } // e.g., Admin, User, etc.
        public string? ProfileImage { get; set; } 
    }
}
