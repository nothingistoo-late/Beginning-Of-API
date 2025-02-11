namespace Final4.DTO.Rating
{
    public class UpdateRating
    {
        public int RatingValue { get; set; }  // Điểm đánh giá (từ 1 đến 5)
        public string Comment { get; set; }  // Bình luận (nếu có)
    }
}
