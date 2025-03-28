﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Final4.Model.Entities
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Khóa chính của Rating

        [Range(1, 5)]  // Giới hạn rating từ 1 đến 5
        public int RatingValue { get; set; }  // Đánh giá (1 đến 5)
        public int LikeCount { get; set; }  // Số lượng lượt thích
        public int DislikeCount { get; set; }  // Số lượng lượt không thích
        public string Comment { get; set; }  // Bình luận (nếu có)
        public DateTime CreatedAt { get; set; }
        public Rating()
        {
            CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time");
        }
        public int OrderDetailId { get; set; }  // Khóa ngoại liên kết với OrderDetail
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }  // Danh sách các bình luận

    }
}
