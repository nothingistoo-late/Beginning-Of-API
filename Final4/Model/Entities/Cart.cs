using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Final4.Model.Entities
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; } // Liên kết với tài khoản
        public virtual Account Account { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>(); // Liên kết nhiều CartItem
    }
}
