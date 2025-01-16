namespace Final4.DTO.Rating
{
    public class AddRating
    {
        public int OrderDetailId { get; set; }     // Khóa ngoại liên kết với OrderDetail (nay có thể null)
        public int RatingValue { get; set; }  // Điểm đánh giá (từ 1 đến 5)
        public string? Comment { get; set; }  // Bình luận (nếu có)
    }
}
