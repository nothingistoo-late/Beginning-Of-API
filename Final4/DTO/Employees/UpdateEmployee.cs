using System.ComponentModel.DataAnnotations;

namespace Final4.DTO.Employees
{
    public class UpdateEmployee
    {
        public required string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public decimal? Salary { get; set; }
    }
}
