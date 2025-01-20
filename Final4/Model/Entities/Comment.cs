using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final4.Model.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }  // Khóa chính của Comment

        [Required]
        public string Content { get; set; }  // Nội dung bình luận

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Thời gian tạo bình luận

        public int RatingId { get; set; }  // Khóa ngoại liên kết với Rating
    }
}
