using System.ComponentModel.DataAnnotations;

namespace Final4.DTO.Order
{
    public class AddOrder
    {
        public string? OrderName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public required string UserEmail { get; set; }  // Khóa ngoại liên kết với User
        public string OrderStatus { get; set; }

    }
}
