using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Entity
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public enum Category
    {
        General,
        SC, // Scheduled Caste
        ST, // Scheduled Tribe
        OBC, // Other Backward Classes
        EWS // Economically Weaker Section
    }

    public class Employee
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }  // Auto-incrementing serial number for counting
        //public int Id { get; set; }
        public string IC_No { get; set; } = string.Empty;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PIS_No { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        [Required]
        public string Cadre { get; set; } = string.Empty;  // Foreign key to Department.Cadre
        [ForeignKey("Cadre")]
        [JsonIgnore]
        public Department? Department { get; set; }

        public string Sub_Cadre { get; set; }
        public string Group { get; set; } = string.Empty;
        
        public string Email { get; set; } =string.Empty;
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? Phone { get; set; }

        public DateOnly? DOB { get; set; } 
        public DateOnly? Date_of_Superannuation { get; set; } 
        public string? Category { get; set; } = string.Empty;
        [Required]
        [EnumDataType(typeof(Gender))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Column(TypeName = "nvarchar(10)")]
        public Gender Gender { get; set; }
        public string? Latest_Qualification { get; set; } = string.Empty;
        public DateOnly? Date_of_Joining_DRDO { get; set; }
        public DateOnly? Date_of_Joining_Lab { get; set; }
        public string? Latest_Discipline { get; set; } = string.Empty;

        public long? UserId { get; set; }
        public User? UserName { get; set; }


    }
    public class UpdateEmployeeDto
    {
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? Sub_Cadre { get; set; }
        public string? Group { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly? Date_of_Superannuation { get; set; }
        public string? Category { get; set; }
        public string? Latest_Qualification { get; set; }
        public string? Latest_Discipline { get; set; }

    }

}
