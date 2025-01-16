using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Final4.Model.Entities
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingId { get; set; }  // Khóa chính của Rating

        [Range(1, 5)]  // Giới hạn rating từ 1 đến 5
        public int RatingValue { get; set; }  // Đánh giá (1 đến 5)

        public string? Comment { get; set; }  // Bình luận (nếu có)

        // Khóa ngoại để liên kết với Order
        [ForeignKey("Order")]
        public int OrderId { get; set; }  // Khóa ngoại liên kết với Order
        [JsonIgnore]
        public virtual Order Order { get; set; }  // Điều hướng đến Order
    }
}
