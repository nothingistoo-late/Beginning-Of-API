using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final4.Model.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }  // Khóa chính
        public required string OrderName { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }  // Khóa ngoại liên kết với User
        public virtual Account? Account { get; set; }  // Điều hướng đến User
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }  // Mối quan hệ nhiều-nhiều với Flower
    }
}
