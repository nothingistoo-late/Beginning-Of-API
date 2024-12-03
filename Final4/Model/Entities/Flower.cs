using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final4.Model.Entities
{
    public class Flower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlowerId { get; set; }
        public required string FlowerName { get; set; }
        public string? FlowerDescription { get; set; }
        public required decimal FlowerPrice { get; set; }
        public required decimal FlowerQuantity { get; set; }
        public required string FlowerImgUrl { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }  // Mối quan hệ nhiều-nhiều với Order

    }
}
