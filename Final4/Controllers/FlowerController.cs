using Final4.Data;
using Final4.DTO.Flower;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [Route("GetAllFlowerBy{FlowerName}")]
        public async Task<IActionResult> GetFlowerByName(string FlowerName)
        {
            var flowers = await _dbcontext.Flowers
                .Where(x => x.FlowerName.ToLower().Contains(FlowerName.ToLower()))
                .ToListAsync();
            return Ok(flowers);
        }

        [HttpPost]
        [Route("AddFlower")]
        public async Task<IActionResult> AddFlower(AddFlower obj)
        {
            Flower
             flower = new()
             {
                 FlowerName = obj.FlowerName,
                 FlowerDescription = obj.FlowerDescription,
                 FlowerQuantity = obj.FlowerQuantity,
                 FlowerPrice = obj.FlowerPrice,
                 ImgUrl = obj.ImgUrl
             };

            _dbcontext.Flowers.Add(flower);
            await _dbcontext.SaveChangesAsync();
            return Ok(flower);
        }
        [HttpPost]
        [Route("UpdateFlowerBy{id}")]
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
                    Flower.ImgUrl = obj.ImgUrl;

                if (obj.FlowerQuantity.HasValue)
                    Flower.FlowerQuantity = obj.FlowerQuantity.Value;

                if (obj.FlowerPrice.HasValue)
                    Flower.FlowerPrice = obj.FlowerPrice.Value;

                await _dbcontext.SaveChangesAsync();
                return Ok(Flower);
            }
        }


        [HttpDelete]
        [Route("DeleteFlowerBy{FlowerId}")]
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
    }
}
