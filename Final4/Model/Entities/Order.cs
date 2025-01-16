using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Final4.Model.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }  // Khóa chính
        public required string OrderName { get; set; }
        public string OrderStatus { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }  // Khóa ngoại liên kết với User
        public virtual Account? Account { get; set; }  // Điều hướng đến User
        [JsonIgnore]

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }  
        public virtual ICollection<Rating> Ratings { get; set; }  // Mối quan hệ 1-nhiều với Rating

    }
}
