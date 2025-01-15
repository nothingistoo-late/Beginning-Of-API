namespace Final4.DTO.Cart
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int AccountId { get; set; }
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>(); // Chỉ trả về thông tin CartItem cần thiết
    }

}
