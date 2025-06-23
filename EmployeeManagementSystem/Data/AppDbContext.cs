using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Entity;
namespace EmployeeManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                        .HasIndex(d => d.Cadre)
                        .IsUnique();


            // Configure the relationship between Employee and Department
            modelBuilder.Entity<Employee>()
                 //.HasOne(e => e.Department)       // An Employee has one Department
                 //.WithMany()                      // A Department can have many Employees
                 //.HasForeignKey(e => e.Cadre)     // The foreign key in Employee is Cadre
                 //.HasPrincipalKey(d => d.Cadre);  // The referenced key in Department is Cadre
                   .HasOne(e => e.Department)
                   .WithMany(d => d.Employees)
                   .HasPrincipalKey(d => d.Cadre) // Use Cadre as the principal key
                   .HasForeignKey(e => e.Cadre)   // Employee.Cadre is the foreign key
                   .OnDelete(DeleteBehavior.Restrict);

            // ✅ Add Employee → User relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.UserName)        // navigation property to User
                .WithMany()                     // no collection of Employees in User
                .HasForeignKey(e => e.UserId)   // Employee.UserId is FK to User.Id
                .OnDelete(DeleteBehavior.Restrict); // optional but recommended
        }
    }
    
}
