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
                 FlowerPrice = obj.FlowerPrice,
                 Availability = obj.Availability,
                 ImgUrl = obj.ImgUrl
             };

            _dbcontext.Flowers.Add(flower);
            await _dbcontext.SaveChangesAsync();
            return Ok(flower);
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
