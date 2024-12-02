namespace Final4.DTO.Order
{
    public class AddOrder
    {
        public string? OrderName { get; set; }
        public required string UserEmail { get; set; }  // Khóa ngoại liên kết với User

    }
}
