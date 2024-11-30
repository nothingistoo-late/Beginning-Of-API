using Final4.Data;
using Final4.DTO.Flower;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAllFlower()
        {
            return Ok(_dbcontext.Flowers.ToList());
        }

        [HttpGet]
        [Route("GetAllFlowerBy{FlowerName}")]
        public IActionResult GetFlowerByName(string FlowerName)
        {
            var listFlower = _dbcontext.Flowers.ToList();
            return Ok(listFlower.FindAll(x => x.FlowerName.ToLower().Contains(FlowerName.ToLower())));
        }

        [HttpPost]
        [Route("AddFlower")]
        public IActionResult AddFlower(AddFlower obj)
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
            _dbcontext.SaveChanges();
            return Ok(flower);
        }

        [HttpDelete]
        [Route("DeleteFlowerBy{FlowerId}")]
        public IActionResult DeleteFlower(int FlowerId)
        {
            var flower = _dbcontext.Flowers.FirstOrDefault(x => x.FlowerId == FlowerId);

            if (flower == null)
            {
                return NotFound($"Flower with ID {FlowerId} not found.");
            }

            _dbcontext.Flowers.Remove(flower);
            _dbcontext.SaveChanges();

            return Ok($"Flower with ID {FlowerId} has been deleted successfully.");
        }
    }
}
