using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Service;

namespace EmployeeManagementSystem.Data
{
    public class DataSeedHelper
    {
        private readonly AppDbContext dbContext;
        private readonly IConfiguration _config;
        private readonly PasswordHelper _passwordHelper;

        public DataSeedHelper(AppDbContext dbContext,
            IConfiguration config,
            PasswordHelper passwordHelper)
        {
            this.dbContext = dbContext;
            _config = config;
            _passwordHelper = passwordHelper;
        }

        public void InsertData()
        {
            if (!dbContext.Departments.Any())
            {
                dbContext.Departments.AddRange(
                    new Department { Cadre = "DRDS" }
                );
                dbContext.SaveChanges();
            }

            if (!dbContext.Employees.Any())
            {
                dbContext.Employees.Add(
                    new Employee
                    {
                        Name = "DR. MEENA MISHRA",
                        Cadre = "DRDS",
                        Sub_Cadre = "DRDS",
                        Designation = "Sc. H & OS (DIRECTOR)",
                        Gender = Gender.Female,
                        IC_No = "IC1234",
                        PIS_No = 1000000001,
                        Group = "Gen",
                        Email = "meena.mishra@example.com",
                        Phone = "1234567890",
                    });
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any())
            {
                var adminPassword = _config["AdminPassword"]
                ?? throw new Exception("AdminPassword secret is missing!");
                //var PasswordHelper = new PasswordHelper();
                dbContext.Users.Add(new User()
                {
                    Username = "Admin",
                    Email = "admin@test.com",
                    Password = _passwordHelper.HashPassword(adminPassword),
                    Role = "Admin",
                });
                dbContext.Users.Add(new User()
                {
                    Username = "Employee1",
                    Email = "emp@test.com",
                    Password = _passwordHelper.HashPassword("12345"),
                    Role = "Employee",
                });

            }
            
            dbContext.SaveChanges();
        }

    }
}
