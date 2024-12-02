using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final4.DTO.Flower
{
    public class AddFlower
    {
        public required string FlowerName { get; set; }
        public string? FlowerDescription { get; set; }
        public decimal? FlowerPrice { get; set; }
        public int FlowerQuantity  { get; set; }
        public required string ImgUrl { get; set; }
    }
}
