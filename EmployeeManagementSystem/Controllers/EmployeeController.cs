using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee, long> employeeRepository;
        private readonly IRepository<Department, string> departmentRepository;
        private readonly Data.IRepository<User,string> userRepo;

        public EmployeeController(
            IRepository<Employee, long> employeeRepository,
            IRepository<Department, string> departmentRepository,
            IRepository<User,string> userRepo)
        {
            this.employeeRepository = employeeRepository;
            this.departmentRepository = departmentRepository;
            this.userRepo = userRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployeeList([FromQuery] SearchOptions searchOption)
        {
            var PagedData = new PagedData<Employee>();
            List<Employee> filterData;
            if (string.IsNullOrEmpty(searchOption.Search))
            {
                PagedData.Data = (await employeeRepository.GetAll()).ToList();
            }
            else
            {
                PagedData.Data = (await employeeRepository.GetAll(e =>
                e.Name.Contains(searchOption.Search) ||
                e.PIS_No.ToString().Contains(searchOption.Search) ||
                e.Email.Contains(searchOption.Search) ||
                e.IC_No.Contains(searchOption.Search) ||
                e.Designation.Contains(searchOption.Search) ||
                e.Cadre.Contains(searchOption.Search) ||
                e.Sub_Cadre.Contains(searchOption.Search) ||
                e.Phone.Contains(searchOption.Search)
                )).ToList();
            }
            PagedData.TotalData = PagedData.Data.Count;
            if (searchOption.PageIndex.HasValue)
            {
                PagedData.Data = PagedData.Data.Skip(searchOption.PageIndex.Value * searchOption.PageSize.Value)
                    .Take(searchOption.PageSize.Value).ToList();
            }
            return Ok(new
            {
                data = PagedData.Data,
                totalData = PagedData.TotalData
            });

        }
        [HttpGet("{PIS_No}")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeById([FromRoute]int PIS_No)
        {
            return Ok(await employeeRepository.FindByIdAsync(PIS_No));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee model)
        {
            // 1. Check for duplicate employee FIRST
            if (await employeeRepository.AnyAsync(e => e.PIS_No == model.PIS_No))
                return Conflict("Employee already exists.");

            // 2. Ensure department exists
            var department = await departmentRepository.FirstOrDefaultAsync(d => d.Cadre == model.Cadre);
            if (department == null)
                return BadRequest($"Department with Cadre '{model.Cadre}' does not exist.");

            // 3. Generate unique email
            string[] nameParts = model.Name.Trim().ToLower().Split(' ');
            string first = nameParts.Length > 0 ? nameParts[0] : "user";
            string last = nameParts.Length > 1 ? nameParts[1] : "emp";
            string departmentPrefix = model.Cadre.ToLower().Replace(" ", "");
            string domain = $"{departmentPrefix}.drdo.gov.in";

            string baseEmail = $"{first}.{last}@{domain}";
            string uniqueEmail = baseEmail;

            int counter = 1;
            while ((await userRepo.FirstOrDefaultAsync(u => u.Email == uniqueEmail)) != null)
            {
                uniqueEmail = $"{first}.{last}{counter}@{domain}";
                counter++;
            }

            // 4. Create User
            var user = new User()
            {
                Username = model.Name,
                Email = uniqueEmail,
                Role = "Employee",
                Password = new PasswordHelper().HashPassword("12345")
            };
            await userRepo.AddAsync(user);
            await userRepo.SaveChangesAsync();

            // 5. Assign user to employee
            model.UserId = user.Id;
            model.Email = uniqueEmail;
            model.Department = department;

            // 6. Save employee
            await employeeRepository.AddAsync(model);
            await employeeRepository.SaveChangesAsync();

            return Ok(new { message = "Employee added successfully.", generatedEmail = uniqueEmail });
        }



        [HttpPut("{PIS_No}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] long PIS_No, [FromBody] UpdateEmployeeDto model)
        {
            var employee = await employeeRepository.FirstOrDefaultAsync(e => e.PIS_No == PIS_No);
            if (employee == null)
                return NotFound($"No employee found with PIS_No {PIS_No}");

            if (model.Name != null)
                employee.Name = model.Name;

            if (model.Designation != null)
                employee.Designation = model.Designation;

            if (model.Sub_Cadre != null)
                employee.Sub_Cadre = model.Sub_Cadre;

            if (model.Group != null)
                employee.Group = model.Group;

            if (model.Email != null)
                employee.Email = model.Email;

            if (model.Phone != null)
                employee.Phone = model.Phone;

            if (model.Date_of_Superannuation.HasValue)
                employee.Date_of_Superannuation = model.Date_of_Superannuation;

            if (model.Category != null)
                employee.Category = model.Category;

            if (model.Latest_Qualification != null)
                employee.Latest_Qualification = model.Latest_Qualification;

            if (model.Latest_Discipline != null)
                employee.Latest_Discipline = model.Latest_Discipline;



            await employeeRepository.UpdateAsync(employee);
            await employeeRepository.SaveChangesAsync();

            return Ok(new { message = "Employee details updated successfully." });
        }


        [HttpDelete("by-pisno/{PIS_No}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployeeByICNo([FromRoute] long PIS_No)
        {
            await employeeRepository.DeleteByPISNoAsync(PIS_No);
            await employeeRepository.SaveChangesAsync();
            return Ok(new { message = $"Employee with PIS_No {PIS_No} deleted successfully." });
        }

    }
}
