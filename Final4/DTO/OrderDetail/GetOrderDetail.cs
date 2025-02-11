namespace Final4.DTO.OrderDetail
{
    public class GetOrderDetail
    {
        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public int FlowerId {  get; set; }
        public int Quantity { get; set; }

        public string Note { get; set; }
    }
}
