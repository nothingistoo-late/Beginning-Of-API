using System.ComponentModel.DataAnnotations;

namespace Final4.DTO.Flower
{
    public class UpdateFlowerDTO
    {
        [Required(ErrorMessage = "FlowerId is required.")]
        public int? FlowerId { get; set; }
        public string? FlowerName { get; set; }
        public string? FlowerDescription { get; set; }
        public decimal? FlowerPrice { get; set; }
        public decimal? FlowerQuantity { get; set; }
        public string? ImgUrl { get; set; }
    }
}
