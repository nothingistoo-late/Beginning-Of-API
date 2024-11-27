using System.ComponentModel.DataAnnotations;

namespace Final4.DTO.Employees
{
    public class UpdateEmployee
    {
        public required string? Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public required string? Email { get; set; }
        public string? Phone { get; set; }
        public required decimal Salary { get; set; }
    }
}
