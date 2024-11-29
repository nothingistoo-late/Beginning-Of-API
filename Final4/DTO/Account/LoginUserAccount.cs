using System.ComponentModel.DataAnnotations;

namespace Final4.DTO.Account
{
    public class LoginUserAccount
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
