namespace Final4.Model.Entities
{
    public class OrderDetail
    {
        public int OrderId { get; set; }      // Khóa ngoại liên kết với Order
        public Order Order { get; set; }     // Điều hướng (navigation property)

        public int FlowerId { get; set; }     // Khóa ngoại liên kết với Flower
        public Flower Flower { get; set; }   // Điều hướng

        public int Quantity { get; set; }    // Số lượng sản phẩm
        public string Note { get; set; }     // Ghi chú (nếu cần)
    }
}
