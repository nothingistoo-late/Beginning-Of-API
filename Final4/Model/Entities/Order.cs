using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final4.Model.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }  // Khóa chính
        public string OrderName { get; set; }
        public int UserId { get; set; }  // Khóa ngoại liên kết với User
        public Account User { get; set; }  // Điều hướng đến User
        public ICollection<OrderDetail> OrderDetails { get; set; }  // Mối quan hệ nhiều-nhiều với Flower
    }
}
