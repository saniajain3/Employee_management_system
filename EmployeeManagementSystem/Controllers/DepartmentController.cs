using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IRepository<Employee, long> employeeRepository;
        private readonly IRepository<Department,string> departmentRepository;

        public DepartmentController(
        IRepository<Department, string> departmentRepository,
          IRepository<Employee, long> employeeRepository)
        {
            this.departmentRepository = departmentRepository;
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment([FromBody] Department model)
        {
            if (string.IsNullOrWhiteSpace(model.Cadre))
                return BadRequest("Cadre is required.");

            var existing = await departmentRepository.FindByIdAsync(model.Cadre);
            if (existing != null)
                return Conflict("Department with this Cadre already exists.");

            var department = new Department
            {
                Cadre = model.Cadre,
                Employees = new List<Employee>() // always initialize
            };

            if (model.Employees != null && model.Employees.Any())
            {
                foreach (var emp in model.Employees)
                {
                    // Only minimally required fields copied here
                    department.Employees.Add(new Employee
                    {
                        IC_No = emp.IC_No,
                        PIS_No = emp.PIS_No,
                        Name = emp.Name,
                        Designation = emp.Designation,
                        Cadre = model.Cadre,
                        Sub_Cadre = emp.Sub_Cadre,
                        Group = emp.Group,
                        Email = emp.Email,
                        Phone = emp.Phone,
                        DOB = emp.DOB,
                        Date_of_Superannuation = emp.Date_of_Superannuation,
                        Category = emp.Category,
                        Gender = emp.Gender,
                        Latest_Qualification = emp.Latest_Qualification,
                        Date_of_Joining_DRDO = emp.Date_of_Joining_DRDO,
                        Date_of_Joining_Lab=emp.Date_of_Joining_Lab,
                        Latest_Discipline = emp.Latest_Discipline
                    });
                }
            }

            await departmentRepository.AddAsync(department);
            await departmentRepository.SaveChangesAsync();

            return Ok(new { message = "Department added successfully." });
        }




        [HttpPut("{cadre}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] string cadre, [FromBody] Department model)
        {
            var department = await departmentRepository.FindByIdAsync(cadre); // Assuming repo supports string PK
            if (department == null) return NotFound();

            departmentRepository.Update(department);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDepartments([FromQuery]SearchOptions options)
        {
            var departments = await departmentRepository.GetAll();
            var list = departments.ToList();
            var pagedData = new PagedData<Department>();
            pagedData.TotalData = list.Count;
            if (options.PageIndex.HasValue)
            {
                pagedData.Data = list.Skip(options.PageIndex.Value * options!.PageSize!.Value).Take(options.PageSize.Value).ToList();
            }
            else
            {
                pagedData.Data = list;
            }
                return Ok(pagedData);
        }

        [HttpDelete("{cadre}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] string cadre)
        {
            await departmentRepository.DeleteAsync(cadre);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }

    }
}

