using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final4.Model.Entities
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }      // Khóa ngoại liên kết với Order
        public virtual Order Order { get; set; }     // Điều hướng (navigation property)

        [ForeignKey("Flower")]
        public int FlowerId { get; set; }     // Khóa ngoại liên kết với Flower
        public virtual Flower Flower { get; set; }   // Điều hướng

        public required int Quantity { get; set; }    // Số lượng sản phẩm
        public string? Note { get; set; }     // Ghi chú (nếu cần)
        public virtual ICollection<Rating> Ratings { get; set; }  // Mỗi OrderDetail có thể có nhiều đánh giá (Ratings)

    }
}
