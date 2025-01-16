using Final4.Data;
using Final4.DTO.Rating;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final4.Controllers
{
    public class RatingController : Controller
    {
        public ApplicationDBContext _dbcontext;
        public RatingController(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost]
        [Route("addRating")]
        public async Task<IActionResult> AddRating([FromBody] AddRating Rating)
        {
            var orderDetail = await _dbcontext.OrderDetails
                                      .Include(od => od.Flower)
                                      .FirstOrDefaultAsync(od => od.OrderDetailId == Rating.OrderDetailId);

            if (orderDetail == null)
            {
                return BadRequest("OrderDetail not found.");
            }

            if (Rating.RatingValue > 5 || Rating.RatingValue < 0)
                return BadRequest("Rating must be between 1 and 5.");

            var rating = new Rating
            {
                OrderDetailId = Rating.OrderDetailId,
                RatingValue = Rating.RatingValue,
                Comment = Rating.Comment
            };

            _dbcontext.Ratings.Add(rating);
            await _dbcontext.SaveChangesAsync();

            return Ok(rating);
        }
    }
}
