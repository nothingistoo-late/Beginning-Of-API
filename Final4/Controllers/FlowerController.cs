﻿using Final4.Data;
using Final4.DTO.Flower;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowerController : ControllerBase
    {
        private ApplicationDBContext _dbcontext;

        public FlowerController(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("GetAllFlower")]
        public async Task<IActionResult> GetAllFlower()
        {
            return Ok(await _dbcontext.Flowers.ToListAsync());
        }

        [HttpGet]
        [Route("GetAllFlowerByFlowerName/{FlowerName}")]
        public async Task<IActionResult> GetFlowerByName(string FlowerName)
        {
            var flowers = await _dbcontext.Flowers
                .Where(x => x.FlowerName.ToLower().Contains(FlowerName.ToLower()))
                .ToListAsync();
            return Ok(flowers);
        }

        [HttpPost]
        [Route("AddFlower")]
        public async Task<IActionResult> AddFlowers(List<AddFlower> flowers)
        {
            var flowersToAdd = new List<Flower>();

            foreach (var obj in flowers)
            {
                var flower = new Flower
                {
                    FlowerName = obj.FlowerName,
                    FlowerDescription = obj.FlowerDescription,
                    FlowerQuantity = obj.FlowerQuantity,
                    FlowerPrice = obj.FlowerPrice,
                    FlowerImgUrl = obj.ImgUrl
                };

                flowersToAdd.Add(flower);
            }

            // Thêm tất cả hoa vào cơ sở dữ liệu
            _dbcontext.Flowers.AddRange(flowersToAdd);
            await _dbcontext.SaveChangesAsync();

            return Ok(flowersToAdd);
        }
        [HttpPost]
        [Route("UpdateFlowerById/{id}")]
        public async Task<IActionResult> UpdateFlower(int id, UpdateFlower obj)
        {
            Flower? Flower = await _dbcontext.Flowers.FindAsync(id);
            if (Flower == null)
                return NotFound("Flower Id Is Not Exit!!\n Please Check FlowerID Again!!");
            else
            {
                // Chỉ cập nhật các trường nếu có giá trị
                if (!string.IsNullOrEmpty(obj.FlowerName))
                    Flower.FlowerName = obj.FlowerName;

                if (!string.IsNullOrEmpty(obj.FlowerDescription))
                    Flower.FlowerDescription = obj.FlowerDescription;

                if (!string.IsNullOrEmpty(obj.ImgUrl))
                    Flower.FlowerImgUrl = obj.ImgUrl;

                if (obj.FlowerQuantity.HasValue)
                    Flower.FlowerQuantity = obj.FlowerQuantity.Value;

                if (obj.FlowerPrice.HasValue)
                    Flower.FlowerPrice = obj.FlowerPrice.Value;

                await _dbcontext.SaveChangesAsync();
                return Ok(Flower);
            }
        }


        [HttpDelete]
        [Route("DeleteFlowerByFlowerId/{FlowerId}")]
        public async Task<IActionResult> DeleteFlower(int FlowerId)
        {
            var flower = await _dbcontext.Flowers.FirstOrDefaultAsync(x => x.FlowerId == FlowerId);
            if (flower == null)
            {
                return NotFound($"Flower with ID {FlowerId} not found.");
            }
            _dbcontext.Flowers.Remove(flower);
            await _dbcontext.SaveChangesAsync();
            return Ok($"Flower with ID {FlowerId} has been deleted successfully.");
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchFlower(string? name, string? description, decimal? priceFrom, decimal? priceTo, decimal? quantity)
        {
            var listFlower = _dbcontext.Flowers.ToList();
            // Lọc theo tên sản phẩm
            if (!string.IsNullOrEmpty(name))
            {
                listFlower = listFlower.Where(p => p.FlowerName.ToLower().Contains(name.ToLower())).ToList();
            }

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(description))
            {
                listFlower = listFlower.Where(p => p.FlowerDescription.ToLower().Contains(description.ToLower())).ToList();
            }

            // Lọc theo giá thấp nhất (>=)
            if (priceFrom.HasValue)
            {
                listFlower = listFlower.Where(p => p.FlowerPrice >= priceFrom.Value).ToList();
            }

            // Lọc theo giá cao nhất (<=)
            if (priceTo.HasValue)
            {
                listFlower = listFlower.Where(p => p.FlowerPrice <= priceTo.Value).ToList();
            }

            if (!listFlower.IsNullOrEmpty())
                return Ok(listFlower);
            else
                return NotFound();
        }
    }
}
