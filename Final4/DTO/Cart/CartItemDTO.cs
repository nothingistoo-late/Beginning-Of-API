namespace Final4.DTO.Cart
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int FlowerId { get; set; }
        public string FlowerName { get; set; } // Lấy tên hoa để hiển thị
        public int Quantity { get; set; }
        public decimal FlowerPrice { get; set; } // Có thể lấy giá hoa nếu cần
    }
}
