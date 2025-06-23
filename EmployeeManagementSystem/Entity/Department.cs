namespace EmployeeManagementSystem.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Department
{
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //public int Id { get; set; }  // Auto-incrementing serial number

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [StringLength(100, MinimumLength = 1)]
    public string Cadre { get; set; } = string.Empty; // remains primary key

    //public string Name { get; set; } = string.Empty;

    [NotMapped]
    public int TotalEmployees => Employees?.Count ?? 0;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
