﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final4.DTO.Flower
{
    public class AddFlowerDTO
    {
        public required string FlowerName { get; set; }
        public string? FlowerDescription { get; set; }
        public required decimal FlowerPrice { get; set; }
        public required decimal FlowerQuantity  { get; set; }
        public required string FlowerImgUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
