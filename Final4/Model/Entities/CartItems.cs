using Final4.Model.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartItemId { get; set; } // ID của CartItem

    [ForeignKey("Cart")]
    public int CartId { get; set; } // ID của giỏ hàng
    public virtual Cart Cart { get; set; } // Liên kết với bảng Cart

    [ForeignKey("Flower")]
    public int FlowerId { get; set; } // ID của sản phẩm (hoa)
    public virtual Flower Flower { get; set; } // Liên kết với bảng Flower
    public int Quantity { get; set; } // Số lượng sản phẩm trong giỏ
}
