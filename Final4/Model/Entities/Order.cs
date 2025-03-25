using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Final4.Model.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Khóa chính
        public required string OrderName { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public Order()
        {
            CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time");
        }

        [ForeignKey("Account")]
        public int AccountId { get; set; }  // Khóa ngoại liên kết với User

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }  

    }
}
