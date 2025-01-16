namespace Final4.DTO.Rating
{
    public class AddRating
    {
        public int OrderId { get; set; }     // Khóa ngoại liên kết với Order (nay có thể null)
        public int RatingValue { get; set; }  // Điểm đánh giá (từ 1 đến 5)
        public string? Comment { get; set; }  // Bình luận (nếu có)
    }
}
